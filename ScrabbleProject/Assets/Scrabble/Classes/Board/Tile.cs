using UnityEngine;
using System.Collections;
using System;

namespace Board
{
	using Ext;
	using Events;
	using Model;

	public class Tile : MonoBehaviour
	{
		[SerializeField] private ETileType m_type;
		[SerializeField] private tk2dSlicedSprite m_skin; 
		[SerializeField] private tk2dTextMesh m_txtType;
		[SerializeField] private TileModel m_tileModel;
		private Model m_model;

		private void Awake ()
		{
			this.Assert<tk2dSlicedSprite>(m_skin, "m_skin must never be null!");
			this.Assert<tk2dTextMesh>(m_txtType, "m_txtType must never be null!");

			// Initialize default tile model
			m_tileModel = new TileModel();
			m_tileModel.Row = -1;
			m_tileModel.Col = -1;
			m_tileModel.Status = ETileStatus.Empty;
			m_tileModel.Letter = null;

			m_model = Model.Instance;

			ScrabbleEvent.Instance.OnTriggerEvent += this.OnEventListened;
		}

		private void Start ()
		{
			this.UpdateTile(m_type);
			this.UpdateSkin();
		}

		private void OnDestroy ()
		{
			this.Type = ETileType.Invalid;
			ScrabbleEvent.Instance.OnTriggerEvent -= this.OnEventListened;
		}

		/// <summary>
		/// True: You can place a letter on it
		/// </summary>
		public bool IsActive 
		{ 
			get { return this.TileModel.Status == ETileStatus.Open; }
		}

		public TileModel TileModel 
		{ 
			get { return m_tileModel; }
			private set { m_tileModel = value; }
		}

		public ETileStatus Status
		{
			get { return m_tileModel.Status; }
			private set { m_tileModel.Status = value; }
		}

		public Rect Rect { get; private set; }

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

		public void PreloadSkin (
			ETileType p_type, 
			int p_row, 
			int p_col
		) {
			this.Assert(p_type != ETileType.Invalid, "m_type is Invalid!");
			m_type = p_type;

			TileModel model = this.TileModel;
			model.Row = p_row;
			model.Col = p_col;
			this.TileModel = model;

			// generate rect
			float offset = BOARD.TILE_OFFSET;
			Vector2 pos = this.transform.position;
			Rect rect = new Rect(pos.x - offset,
			                     pos.y - offset,
			                     pos.x + offset,
			                     pos.y + offset);
			this.Rect = rect;
		}

		public void Activate ()
		{
			TileModel model = this.TileModel;
			model.Letter = null;
			model.Status = ETileStatus.Open;
			this.TileModel = model;
			m_skin.color = Color.white;
		}

		public void Deactivate ()
		{
			TileModel model = this.TileModel;
			model.Status = ETileStatus.Empty;
			this.TileModel = model;
			m_skin.color = Color.gray;
		}

		private void UpdateSkin ()
		{
			if (m_type == ETileType.Invalid) { return; }
			m_skin.spriteId = m_skin.GetSpriteIdByName(m_model.Board.TileSprite(m_type));
			m_txtType.text = m_model.Board.TileDisplay(m_type);
		}

		private void OnEventListened (EEvents p_type, IEventData p_data)
		{
			switch (p_type)
			{
				case EEvents.OnSnapped:
				{
					SnapEvent snap = (SnapEvent)p_data;
					Tile tile = snap.Data<Tile>(SnapEvent.TILE);
					ELetter eletter = snap.Data<ELetter>(SnapEvent.LETTER);
					Letter letter = m_model.Rack.Letter(eletter);
					
					if (tile.TileModel.Row == m_tileModel.Row
				    &&	tile.TileModel.Col == m_tileModel.Col
				   	) {
						this.Log(Tags.Log, "Tile::OnEventListened Tile:{0} Letter:{1}", tile, letter);
						letter.transform.position = this.transform.position;
						letter.transform.localScale = new Vector3(BOARD.TILE_OFFSET, BOARD.TILE_OFFSET, 0f);
						m_tileModel.Letter = letter;
						m_tileModel.Status = ETileStatus.Occupied;
					}
				}
				break;
			}
		}
	}
}