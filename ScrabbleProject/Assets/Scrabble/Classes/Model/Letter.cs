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
		private Model m_model;

		private void Awake ()
		{
			this.Assert<tk2dSlicedSprite>(m_skin, "m_skin must never be null!");
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
		
		private void UpdateSkin ()
		{
			if (m_letter == ELetter.Invalid) { return; }
			//m_skin.spriteId = m_skin.GetSpriteIdByName(m_model.Board.TileSprite(m_letter));
		}
	}
}