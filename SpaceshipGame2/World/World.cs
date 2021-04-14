using PixelEngine;
using SpaceshipGame2.Utility;
using SpaceshipGame2.World.Entities;
using SpaceshipGame2.World.Graphics;
using System.Collections.Generic;
using System.Linq;

using vf = SpaceshipGame2.Utility.Vector2;
using vi = SpaceshipGame2.Utility.Vector2i;

namespace SpaceshipGame2.World {
	class World : IUpdateable {
		#region vars camera
		public vf cameraPosition_w = (0, 0);
		public vf cameraSize_s;
		public float cameraZoom = 1;
		public bool cameraClear = true;
		public bool cameraDrawDebug = false;
		public Pixel cameraClearColour = Pixel.Presets.Black;
		vf cameraPanStart;
		#endregion vars camera

		#region chunking / world objects
		public Dictionary<vi, Chunk> chunks;
		public List<WorldObject> allObjects;

		/// <summary>
		/// Add an object to the world
		/// </summary>
		public void AddObject(WorldObject worldObject) {
			allObjects.Add(worldObject);

			vi chunkPos_c = Chunk.ChunkFromWorldPosition_c(worldObject.position_w);
			ExistingOrNewChunk(chunkPos_c);
			chunks[chunkPos_c].worldObjects.Add(worldObject);
			worldObject.chunk = chunks[chunkPos_c];
		}

		/// <summary>
		/// Remove an object from the world
		/// </summary>
		public void RemoveObject(WorldObject worldObject) {
			if (worldObject.chunk != null) {
				allObjects.Remove(worldObject);
				worldObject.chunk.worldObjects.Remove(worldObject);
				RemoveChunkIfEmpty(worldObject.chunk);
			}
		}

		List<WorldObject> deletionRegister = new List<WorldObject>();
		/// <summary>
		/// Register a world object for deletion after updating for this frame is done
		/// </summary>
		public void RegisterForDeletion(WorldObject worldObject) {
			deletionRegister.Add(worldObject);
		}

		/// <summary>
		/// Create chunk if it does not exist, or [TODO] load chunk data from disk
		/// </summary>
		/// <param name="chunkPos_c">chunk coordinates to load</param>
		public Chunk ExistingOrNewChunk(vi chunkPos_c) {
			if (chunks.ContainsKey(chunkPos_c)) return chunks[chunkPos_c];

			bool chunkExistsOnDisk = false; // [TODO] implement loading from disk
			if (chunkExistsOnDisk) {
				// [TODO] load
				return null;
			} else {
				// create new chunk
				chunks[chunkPos_c] = new Chunk(this, chunkPos_c);
				return chunks[chunkPos_c];
			}
		}

		/// <summary>
		/// [TODO] Save all chunk data to disk
		/// </summary>
		public void SaveChunk(Chunk chunk) {

		}

		/// <summary>
		/// Clears all world objects in a chunk from the object list
		/// </summary>
		void ClearInhabitantsFromObjectList(Chunk chunk) {
			foreach (WorldObject worldObject in chunk.worldObjects.ToArray())
				RemoveObject(worldObject);
		}

		/// <summary>
		/// Unloads a chunk, [TODO] if it contains objects, saves the chunk to disk.
		/// </summary>
		public void UnloadChunk(Chunk chunk) {
			RemoveChunkIfEmpty(chunk);

			// if it survived above line of code, it has data
			if (chunks.ContainsKey(chunk.chunkPosition_c)) {
				SaveChunk(chunk); // save
				ClearInhabitantsFromObjectList(chunk);
				chunks.Remove(chunk.chunkPosition_c); // now delete
			}
		}

		/// <summary>
		/// Remove a chunk if it has no objects in it
		/// </summary>
		/// <param name="chunk">chunk to check</param>
		/// <returns></returns>
		public void RemoveChunkIfEmpty(Chunk chunk) {
			if (chunk.worldObjects.Count == 0)
				chunks.Remove(chunk.chunkPosition_c);
		}

		/// <summary>
		/// Loop over all chunks and delete those with no relevant information in them
		/// </summary>
		void RemoveAllEmptyChunks() {
			List<vi> removals = new List<vi>();

			foreach (Chunk chunk in chunks.Values)
				if (chunk.worldObjects.Where(x => x.shouldBeSaved == true).Count() == 0)
					removals.Add(chunk.chunkPosition_c);

			foreach (vi chunkPos_c in removals)
				chunks.Remove(chunkPos_c);
		}

		/// <summary>
		/// Get all world objects in a collection of chunks
		/// </summary>
		IEnumerable<WorldObject> GetObjects(IEnumerable<Chunk> chunks) {
			foreach (Chunk chunk in chunks)
				foreach (WorldObject o in chunk.worldObjects)
					yield return o;
		}

		/// <summary>
		/// Get all chunks the camera can see
		/// </summary>
		IEnumerable<Chunk> GetVisibleChunks() {
			vf cameraTopLeft_w = cameraPosition_w;
			vf cameraBottomRight_w = cameraTopLeft_w + (WorldLength(cameraSize_s.x), WorldLength(cameraSize_s.y));
			AABB cameraRegion_w = new AABB(cameraTopLeft_w, cameraBottomRight_w);

			return GetChunksInRegion(cameraRegion_w, 0);
		}

		/// <summary>
		/// Get all chunks in a world AABB
		/// </summary>
		/// <param name="region_w">region in world space</param>
		/// <param name="border">extra chunks around the resultant chunks to also return</param>
		/// <returns></returns>
		IEnumerable<Chunk> GetChunksInRegion(AABB region_w, int border = 1) {
			vi topLeft_c = Chunk.ChunkFromWorldPosition_c(region_w.topLeft);
			vi bottomRight_c = Chunk.ChunkFromWorldPosition_c(region_w.bottomRight);

			for (int x_c = topLeft_c.x - border; x_c <= bottomRight_c.x + border; x_c++) {
				for (int y_c = topLeft_c.y - border; y_c <= bottomRight_c.y + border; y_c++) {
					if (!chunks.ContainsKey((x_c, y_c))) chunks[(x_c, y_c)] = new Chunk(this, (x_c, y_c));
					if (chunks.ContainsKey((x_c, y_c))) yield return chunks[(x_c, y_c)];
				}
			}
		}
		#endregion chunking / world objects

		#region special entities
		Entity player;
		#endregion special entities

		float shootTimer;
		public void Update(SpaceGame target, float elapsed) {
			if (target.inputManager.ActionPressed("up")) player.ApplyForceAtAngle(1, player.rotation);
			if (target.inputManager.ActionPressed("down")) player.ApplyForceAtAngle(-1, player.rotation);
			if (target.inputManager.ActionPressed("left")) player.ApplyRotationalForce(-0.1f);
			if (target.inputManager.ActionPressed("right")) player.ApplyRotationalForce(0.1f);

			shootTimer -= elapsed;
			if (target.GetKey(Key.Space).Down && shootTimer <= 0) {
				Entity bullet = new Bullet(vf.Along(player.position_w, 20, -player.rotation + (float)System.Math.PI / 2));
				AddObject(bullet);
				bullet.rotation = player.rotation;
				bullet.vel = player.vel + vf.Along(default, 1, -player.rotation + (float)System.Math.PI / 2);
				bullet.graphics = MultiPolygon.FromString("[name(bullet)v(-0.2,-0.5)v(0.2,-0.5)v(0.2,0.5)v(-0.2,0.5)colour(0,255,0)scale(10)]");
				shootTimer = 0.1f;
			}

			CameraCenterOnEntity(player);
			CameraZoomBy(1 + (float)target.MouseScroll / 10, cameraSize_s / 2);

			IEnumerable<WorldObject> currentlyVisibleWorldObjects = GetObjects(GetVisibleChunks());

			// update all entities
			foreach (WorldObject wo in allObjects) {
				if (wo is Entity e)
					e.Update(target, elapsed);
			}

			// delete all objects registered for deletion
			foreach (WorldObject worldObjectForDeletion in deletionRegister)
				RemoveObject(worldObjectForDeletion);

			// make sure all world objects are in the correct chunk
			List<WorldObject> misplacedWorldObjects = new List<WorldObject>();
			foreach (WorldObject worldObject in allObjects) {
				vi chunkPos_c = Chunk.ChunkOrigin_w(worldObject.position_w);
				if (chunkPos_c != worldObject.chunk.chunkPosition_c) {
					misplacedWorldObjects.Add(worldObject);
				}
			}
			foreach (WorldObject misplacedWorldObject in misplacedWorldObjects) {
				misplacedWorldObject.chunk.worldObjects.Remove(misplacedWorldObject);
				misplacedWorldObject.chunk = ExistingOrNewChunk(Chunk.ChunkFromWorldPosition_c(misplacedWorldObject.position_w));
				chunks[Chunk.ChunkFromWorldPosition_c(misplacedWorldObject.position_w)].worldObjects.Add(misplacedWorldObject);
			}

			RemoveAllEmptyChunks();
		}

		public void Draw(Game target) {
			if (cameraClear) target.Clear(cameraClearColour);

			IEnumerable<WorldObject> currentlyVisibleWorldObjects = GetObjects(GetVisibleChunks());
			target.DrawText(new Point(10, 20), $"{currentlyVisibleWorldObjects.Contains(player)}", Pixel.Presets.Cyan);

			foreach (WorldObject worldObject in currentlyVisibleWorldObjects) {
				worldObject.Draw(target, this);
			}
		}

		#region methods camera
		public void CameraMove(vf amount_w) =>
			cameraPosition_w -= amount_w / cameraZoom;
		public void CameraCenterOn(vf location_w) =>
			cameraPosition_w = location_w - cameraSize_s / cameraZoom / 2;
		public void CameraCenterOnEntity(Entity e) =>
			CameraCenterOn(e.position_w);

		public float WorldLength(float length) =>
			length * 1 / cameraZoom;
		public float ScreenLength(float length) =>
			length * cameraZoom;

		/// <summary>
		/// Screen space -> World space
		/// </summary>
		public vf WorldPoint(vf screenPoint) =>
			(screenPoint / cameraZoom) + cameraPosition_w;
		/// <summary>
		/// World space -> Screen Space
		/// </summary>
		public vf ScreenPoint(vf worldPoint) =>
			(worldPoint - cameraPosition_w) * cameraZoom;

		public void CameraStartPan(vf loc) =>
			cameraPanStart = loc;
		public void CameraContinuePan(vf loc) {
			CameraMove(loc - cameraPanStart);
			cameraPanStart = loc;
		}

		public void CameraZoomBy(float amount, vf focus_s) {
			vf start_w = WorldPoint(focus_s);
			cameraZoom *= amount;
			vf end_w = WorldPoint(focus_s);
			cameraPosition_w += start_w - end_w;
		}
		#endregion methods camera

		public World(vf position_w, vf size_s) {
			cameraPosition_w = position_w;
			cameraSize_s = size_s;
			CameraCenterOn(default); // center camera on (0, 0)

			chunks = new Dictionary<vi, Chunk>();
			allObjects = new List<WorldObject>();

			player = new Entity((0, 0));
			AddObject(player);
		}
	}
}

