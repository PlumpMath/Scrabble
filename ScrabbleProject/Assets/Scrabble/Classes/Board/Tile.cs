using UnityEngine;
using System.Collections;

namespace Board
{
	using Ext;
	using Model;

	public class Tile : MonoBehaviour 
	{
		[SerializeField] private ETileType m_type;
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
			this.Type = ETileType.Invalid;
		}

		/// <summary>
		/// True: You can place a letter on it
		/// </summary>
		public bool IsActive { get; private set; }

		// TODO: Add letter here

		public ETileType Type
		{ 
			get { return m_type; } 
			private set { m_type = value; }
		}

		public void UpdateTile (ETileType p_type)
		{
			this.Assert(p_type != ETileType.Invalid, "m_type is Invalid!");
			if (m_type == p_type) { return; }
			m_type = p_type;
			this.UpdateSkin();
		}

		public void PreloadSkin (ETileType p_type)
		{
			this.Assert(p_type != ETileType.Invalid, "m_type is Invalid!");
			m_type = p_type;
		}

		private void UpdateSkin ()
		{
			if (m_type == ETileType.Invalid) { return; }
			m_skin.spriteId = m_skin.GetSpriteIdByName(m_model.Board.TileSprite(m_type));
		}
	}
}