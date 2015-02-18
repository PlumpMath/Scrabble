using UnityEngine;
using System.Collections;

namespace Board
{
	using Ext;
	using Model;

	public class Tile : MonoBehaviour 
	{
		[SerializeField] private TileType m_type;
		[SerializeField] private tk2dSlicedSprite m_skin; 
		private Model m_model;

		private void Awake ()
		{
			this.Assert<tk2dSlicedSprite>(m_skin, "m_skin must never be null!");
			m_model = Model.Instance;
		}

		private void Start ()
		{
			this.UpdateTile(m_type);
			this.UpdateSkin();
		}

		private void OnDestroy ()
		{
			this.Type = TileType.Invalid;
		}

		public TileType Type
		{ 
			get { return m_type; } 
			private set { m_type = value; }
		}

		public void UpdateTile (TileType p_type)
		{
			this.Assert(p_type != TileType.Invalid, "m_type is Invalid!");
			if (m_type == p_type) { return; }
			m_type = p_type;
			this.UpdateSkin();
		}

		public void PreloadSkin (TileType p_type)
		{
			this.Assert(p_type != TileType.Invalid, "m_type is Invalid!");
			m_type = p_type;
		}

		private void UpdateSkin ()
		{
			if (m_type == TileType.Invalid) { return; }
			m_skin.spriteId = m_skin.GetSpriteIdByName(m_model.Board.TileSprite(m_type));
		}
	}
}