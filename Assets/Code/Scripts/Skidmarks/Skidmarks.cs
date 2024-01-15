using UnityEngine;
using UnityEngine.Rendering;

namespace ITPRO.Rover.Skid
{
	public class Skidmarks : MonoBehaviour
	{
		[SerializeField] Material skidmarksMaterial; // Material for the skidmarks to use

		// END INSPECTOR SETTINGS

		const int MAX_MARKS = 8192; // Max number of marks total for everyone together
		const float MARK_WIDTH = 0.35f; // Width of the skidmarks. Should match the width of the wheels
		const float GROUND_OFFSET = 0.02f; // Distance above surface in metres

		const float MIN_DISTANCE = 0.8f; // Distance between skid texture sections in metres. Bigger = better performance, less smooth

		const float MIN_SQR_DISTANCE = MIN_DISTANCE * MIN_DISTANCE;
		const float MAX_OPACITY = 1.0f; // Max skidmark opacity

		// Info for each mark created. Needed to generate the correct mesh


		class MarkSection
		{
			public Vector3 Pos = Vector3.zero;
			public Vector3 Normal = Vector3.zero;
			public Vector4 Tangent = Vector4.zero;
			public Vector3 Posl = Vector3.zero;
			public Vector3 Posr = Vector3.zero;
			public Color32 Colour;
			public int LastIndex;
		};

		private int _markIndex;
		private MarkSection[] _skidmarks;
		private Mesh _marksMesh;
		private MeshRenderer _meshRenderer;
		private MeshFilter _meshFilter;

		private Vector3[] _vertices;
		private Vector3[] _normals;
		private Vector4[] _tangents;
		private Color32[] _colors;
		private Vector2[] _uvs;
		private int[] _triangles;

		private bool meshUpdated;
		private bool haveSetBounds;

		private Color32 _black = Color.black;
		public static Skidmarks instance;

		// #### UNITY INTERNAL METHODS ####

		protected void Awake()
		{
			if (transform.position != Vector3.zero)
			{
				Debug.LogWarning("Skidmarks.cs transform must be at 0,0,0. Setting it to zero now.");
				transform.position = Vector3.zero;
				transform.rotation = Quaternion.identity;
			}

			if (instance == null)
			{
				instance = this;
			}
			else
			{
				Destroy(this);
			}
		}

		protected void Start()
		{
			// Generate a fixed array of skidmarks
			_skidmarks = new MarkSection[MAX_MARKS];
			for (int i = 0; i < MAX_MARKS; i++)
			{
				_skidmarks[i] = new MarkSection();
			}

			_meshFilter = GetComponent<MeshFilter>();
			_meshRenderer = GetComponent<MeshRenderer>();
			if (_meshRenderer == null)
			{
				_meshRenderer = gameObject.AddComponent<MeshRenderer>();
			}

			_marksMesh = new Mesh();
			_marksMesh.MarkDynamic();
			if (_meshFilter == null)
			{
				_meshFilter = gameObject.AddComponent<MeshFilter>();
			}

			_meshFilter.sharedMesh = _marksMesh;

			_vertices = new Vector3[MAX_MARKS * 4];
			_normals = new Vector3[MAX_MARKS * 4];
			_tangents = new Vector4[MAX_MARKS * 4];
			_colors = new Color32[MAX_MARKS * 4];
			_uvs = new Vector2[MAX_MARKS * 4];
			_triangles = new int[MAX_MARKS * 6];

			_meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			_meshRenderer.receiveShadows = false;
			_meshRenderer.material = skidmarksMaterial;
			_meshRenderer.lightProbeUsage = LightProbeUsage.Off;
		}

		protected void LateUpdate()
		{
			if (!meshUpdated) return;
			meshUpdated = false;

			// Reassign the mesh if it's changed this frame
			_marksMesh.vertices = _vertices;
			_marksMesh.normals = _normals;
			_marksMesh.tangents = _tangents;
			_marksMesh.triangles = _triangles;
			_marksMesh.colors32 = _colors;
			_marksMesh.uv = _uvs;

			if (!haveSetBounds)
			{
				// Could use RecalculateBounds here each frame instead, but it uses about 0.1-0.2ms each time
				// Save time by just making the mesh bounds huge, so the skidmarks will always draw
				// Not sure why I only need to do this once, yet can't do it in Start (it resets to zero)
				_marksMesh.bounds = new Bounds(new Vector3(0, 0, 0), new Vector3(10000, 10000, 10000));
				haveSetBounds = true;
			}

			_meshFilter.sharedMesh = _marksMesh;
		}

		// #### PUBLIC METHODS ####

		// Function called by the wheel that's skidding. Sets the intensity of the skidmark section, in the default black.
		public int AddSkidMark(Vector3 pos, Vector3 normal, float opacity, int lastIndex)
		{
			if (opacity > 1) opacity = 1.0f;
			else if (opacity < 0) return -1;

			_black.a = (byte) (opacity * 255);
			return AddSkidMark(pos, normal, _black, lastIndex);
		}

		// Function called by the wheel that's skidding. Sets the colour and intensity of the skidmark section.
		public int AddSkidMark(Vector3 pos, Vector3 normal, Color32 colour, int lastIndex)
		{
			if (colour.a == 0) return -1; // No point in continuing if it's invisible	

			MarkSection lastSection = null;
			Vector3 distAndDirection = Vector3.zero;
			Vector3 newPos = pos + normal * GROUND_OFFSET;
			if (lastIndex != -1)
			{
				lastSection = _skidmarks[lastIndex];
				distAndDirection = newPos - lastSection.Pos;
				if (distAndDirection.sqrMagnitude < MIN_SQR_DISTANCE)
				{
					return lastIndex;
				}

				// Fixes an awkward bug:
				// - Car draws skidmark, e.g. index 50 with last index 40.
				// - Skidmark markIndex loops around, and other car overwrites index 50
				// - Car draws skidmark, e.g. index 45. Last index was 40, but now 40 is different, changed by someone else.
				// This makes sure we ignore the last index if the distance looks wrong
				if (distAndDirection.sqrMagnitude > MIN_SQR_DISTANCE * 10)
				{
					lastIndex = -1;
					lastSection = null;
				}
			}

			colour.a = (byte) (colour.a * MAX_OPACITY);

			MarkSection curSection = _skidmarks[_markIndex];

			curSection.Pos = newPos;
			curSection.Normal = normal;
			curSection.Colour = colour;
			curSection.LastIndex = lastIndex;

			if (lastSection != null)
			{
				Vector3 xDirection = Vector3.Cross(distAndDirection, normal).normalized;
				curSection.Posl = curSection.Pos + xDirection * MARK_WIDTH * 0.5f;
				curSection.Posr = curSection.Pos - xDirection * MARK_WIDTH * 0.5f;
				curSection.Tangent = new Vector4(xDirection.x, xDirection.y, xDirection.z, 1);

				if (lastSection.LastIndex == -1)
				{
					lastSection.Tangent = curSection.Tangent;
					lastSection.Posl = curSection.Pos + xDirection * MARK_WIDTH * 0.5f;
					lastSection.Posr = curSection.Pos - xDirection * MARK_WIDTH * 0.5f;
				}
			}

			UpdateSkidmarksMesh();

			int curIndex = _markIndex;
			// Update circular index
			_markIndex = ++_markIndex % MAX_MARKS;

			return curIndex;
		}

		// #### PROTECTED/PRIVATE METHODS ####

		// Update part of the mesh for the current markIndex
		void UpdateSkidmarksMesh()
		{
			MarkSection curr = _skidmarks[_markIndex];

			// Nothing to connect to yet
			if (curr.LastIndex == -1) return;

			MarkSection last = _skidmarks[curr.LastIndex];
			_vertices[_markIndex * 4 + 0] = last.Posl;
			_vertices[_markIndex * 4 + 1] = last.Posr;
			_vertices[_markIndex * 4 + 2] = curr.Posl;
			_vertices[_markIndex * 4 + 3] = curr.Posr;

			_normals[_markIndex * 4 + 0] = last.Normal;
			_normals[_markIndex * 4 + 1] = last.Normal;
			_normals[_markIndex * 4 + 2] = curr.Normal;
			_normals[_markIndex * 4 + 3] = curr.Normal;

			_tangents[_markIndex * 4 + 0] = last.Tangent;
			_tangents[_markIndex * 4 + 1] = last.Tangent;
			_tangents[_markIndex * 4 + 2] = curr.Tangent;
			_tangents[_markIndex * 4 + 3] = curr.Tangent;

			_colors[_markIndex * 4 + 0] = last.Colour;
			_colors[_markIndex * 4 + 1] = last.Colour;
			_colors[_markIndex * 4 + 2] = curr.Colour;
			_colors[_markIndex * 4 + 3] = curr.Colour;

			_uvs[_markIndex * 4 + 0] = new Vector2(0, 0);
			_uvs[_markIndex * 4 + 1] = new Vector2(1, 0);
			_uvs[_markIndex * 4 + 2] = new Vector2(0, 1);
			_uvs[_markIndex * 4 + 3] = new Vector2(1, 1);

			_triangles[_markIndex * 6 + 0] = _markIndex * 4 + 0;
			_triangles[_markIndex * 6 + 2] = _markIndex * 4 + 1;
			_triangles[_markIndex * 6 + 1] = _markIndex * 4 + 2;

			_triangles[_markIndex * 6 + 3] = _markIndex * 4 + 2;
			_triangles[_markIndex * 6 + 5] = _markIndex * 4 + 1;
			_triangles[_markIndex * 6 + 4] = _markIndex * 4 + 3;

			meshUpdated = true;
		}
	}
}