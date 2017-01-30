using UnityEngine;
using System.Collections;
using Tacticsoft;
using UnityEngine.UI;



//Inherit from TableViewCell instead of MonoBehavior to use the GameObject
//containing this component as a cell in a TableView
public class GuessResultCell : TableViewCell
{
	public Text m_rowNumberText;
	public Text m_numberText;
	public Text m_PozitiveText;
	public Text m_NegativeText;

	public Text m_rowNumber2Text;
	public Text m_number2Text;
	public Image m_PozitiveImage;
	public Image m_Pozitive2Image;
	public Text m_Pozitive2Text;
	public Image m_Negative2Image;
	public Image m_NegativeImage;
	public Text m_Negative2Text;

	public void SetRowNumber(int rowNumber) {
		m_rowNumberText.text = rowNumber.ToString()+".";
	}

	public void SetRowNumber2(int rowNumber) {
		m_rowNumber2Text.text = rowNumber.ToString()+".";
	}

	public void SetNumber(int number) {
		if (GameData.Instance.appModeType == 3) { 
			m_numberText.text = number.ToString ().PadLeft (3, '0');
		} else {
			m_numberText.text = number.ToString ().PadLeft (4, '0');
		}
	}

	public void SetNumber2(int number) {
		if (GameData.Instance.appModeType == 3) { 
			m_number2Text.text = number.ToString ().PadLeft (3, '0');
		} else {
			m_number2Text.text = number.ToString ().PadLeft (4, '0');
		}
	}

	public void SetPozitive(int pozitive) {
		m_PozitiveText.text = pozitive.ToString();
	}

	public void SetPozitive2(int pozitive) {
		m_Pozitive2Text.text = pozitive.ToString();
	}

	public void SetPozitiveImage() {
		m_PozitiveImage.gameObject.SetActive(true);
	}

	public void SetPozitiveImage2() {
		m_Pozitive2Image.gameObject.SetActive(true);
	}

	public void SetNegative(int negative) {
		m_NegativeText.text = negative.ToString();
	}

	public void SetNegative2(int negative) {
		m_Negative2Text.text = negative.ToString();
	}

	public void SetNegativeImage() {
		m_NegativeImage.gameObject.SetActive(true);
	}

	public void SetNegativeImage2() {
		m_Negative2Image.gameObject.SetActive(true);
	}

	private int m_numTimesBecameVisible;
	public bool NotifyBecameVisible(int rowCount) {
		m_numTimesBecameVisible++;
		if (m_numTimesBecameVisible == rowCount)
			return true;
		else
			return false;

		
//		m_visibleCountText.text = "# rows this cell showed : " + m_numTimesBecameVisible.ToString();
	}

	
}
