using UnityEngine;
using System.Collections;

namespace Board
{
	using Ext;
	using Model;

	public class Letter : MonoBehaviour 
	{
		[SerializeField] private ELetter m_letter;
		[SerializeField] private tk2dSlicedSprite m_skin; 
		[SerializeField] private tk2dTextMesh m_txtText;
		[SerializeField] private tk2dTextMesh m_txtPoints;
		[SerializeField] private Vector3 m_originalPos;
		private Model m_model;

		private void Awake ()
		{
			this.Assert<tk2dSlicedSprite>(m_skin, "m_skin must never be null!");
			this.Assert<tk2dTextMesh>(m_txtText, "m_txtText must never be null!");
			this.Assert<tk2dTextMesh>(m_txtPoints, "m_txtPoints must never be null!");
			this.Tile = null;
			m_model = Model.Instance;
		}
		
		private void Start ()
		{
			this.UpdateLetter(m_letter);
			this.UpdateSkin();
		}
		
		private void OnDestroy ()
		{
			this.Type = ELetter.Invalid;
		}
		
		public ELetter Type
		{ 
			get { return m_letter; } 
			private set { m_letter = value; }
		}

		public Tile Tile { get; set; }

		public void UpdateLetter (ELetter p_type)
		{
			this.Assert(p_type != ELetter.Invalid, "m_letter is Invalid!");
			if (m_letter == p_type) { return; }
			m_letter = p_type;
			this.UpdateSkin();
		}
		
		public void PreloadSkin (ELetter p_type)
		{
			this.Assert(p_type != ELetter.Invalid, "m_letter is Invalid!");
			m_letter = p_type;
		}

		public void Ready ()
		{
			m_originalPos = this.transform.position;
		}

		/// <summary>
		/// Dragging is unsuccessful
		/// </summary>
		public void Reset ()
		{
			this.transform.position = m_originalPos;
		}
		
		private void UpdateSkin ()
		{
			if (m_letter == ELetter.Invalid) { return; }
			//m_skin.spriteId = m_skin.GetSpriteIdByName(m_model.Board.TileSprite(m_letter));
			m_txtText.text = m_model.Board.LetterText(m_letter);
			m_txtPoints.text = "" + m_model.Board.LetterPoints(m_letter);
		}
	}
}