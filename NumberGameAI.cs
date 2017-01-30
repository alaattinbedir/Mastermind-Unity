using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NumberGameAI {

	public Number myNumber;
	public Number opponentNumber;
	public Number computerNumber;
	public Number numberFounded;
	public List<Number> numbersPool;
	public List<Number> myNumbersPool;
	public ArrayList doubleDigits;
	public ArrayList myDoubleDigits;
	private ArrayList foundedDigits;
	private ArrayList notAvailableDigits;
	private ArrayList availableDigits;
	private bool tumSayilarBirTahmindeBulundu;

	private static NumberGameAI _instance = null;
	
	private NumberGameAI() {
		// Anything to init would go here

		doubleDigits = new ArrayList();
		doubleDigits.Add (0);
		doubleDigits.Add (1);
		doubleDigits.Add (2);
		doubleDigits.Add (3);
		doubleDigits.Add (4);
		doubleDigits.Add (5);
		doubleDigits.Add (6);
		doubleDigits.Add (7);
		doubleDigits.Add (8);
		doubleDigits.Add (9);


		myDoubleDigits = new ArrayList();
		myDoubleDigits.Add (0);
		myDoubleDigits.Add (1);
		myDoubleDigits.Add (2);
		myDoubleDigits.Add (3);
		myDoubleDigits.Add (4);
		myDoubleDigits.Add (5);
		myDoubleDigits.Add (6);
		myDoubleDigits.Add (7);
		myDoubleDigits.Add (8);
		myDoubleDigits.Add (9);


		notAvailableDigits = new ArrayList ();
		foundedDigits = new ArrayList ();
		availableDigits = new ArrayList ();
		tumSayilarBirTahmindeBulundu = false;
		computerNumber = new Number ();
	}
	
	public static NumberGameAI Instance {
		get {
			if (_instance == null) {
				_instance = new NumberGameAI();
			}
			return _instance;
		}
	}


	public void createRandomComputerNumber3(){

		ArrayList digitsArray = new ArrayList();
		digitsArray.Add (0);
		digitsArray.Add (1);
		digitsArray.Add (2);
		digitsArray.Add (3);
		digitsArray.Add (4);
		digitsArray.Add (5);
		digitsArray.Add (6);
		digitsArray.Add (7);
		digitsArray.Add (8);
		digitsArray.Add (9);


		int firstNumberPosition = Random.Range(0,9);
		int firstNumber = (int)digitsArray[firstNumberPosition];
		digitsArray.RemoveAt(firstNumberPosition);
		
		int secondNumberPosition = Random.Range(0,8);
		int secondNumber = (int)digitsArray[secondNumberPosition];
		digitsArray.RemoveAt(secondNumberPosition);
		
		int thirdNumberPosition = Random.Range(0,7);
		int thirdNumber = (int)digitsArray[thirdNumberPosition];
		digitsArray.RemoveAt(thirdNumberPosition);
		
		Debug.Log("Computer number:: "+ firstNumber.ToString() + " " + secondNumber.ToString() + " " +thirdNumber.ToString());

		this.computerNumber = new Number();
		this.computerNumber.FirstNumber = firstNumber;
		this.computerNumber.SecondNumber = secondNumber;
		this.computerNumber.ThirdNumber = thirdNumber;

		return;
	}

	public void createRandomComputerNumber4(){

		ArrayList digitsArray = new ArrayList();
		digitsArray.Add (0);
		digitsArray.Add (1);
		digitsArray.Add (2);
		digitsArray.Add (3);
		digitsArray.Add (4);
		digitsArray.Add (5);
		digitsArray.Add (6);
		digitsArray.Add (7);
		digitsArray.Add (8);
		digitsArray.Add (9);


		int firstNumberPosition = Random.Range(0,9);
		int firstNumber = (int)digitsArray[firstNumberPosition];
		digitsArray.RemoveAt(firstNumberPosition);

		int secondNumberPosition = Random.Range(0,8);
		int secondNumber = (int)digitsArray[secondNumberPosition];
		digitsArray.RemoveAt(secondNumberPosition);

		int thirdNumberPosition = Random.Range(0,7);
		int thirdNumber = (int)digitsArray[thirdNumberPosition];
		digitsArray.RemoveAt(thirdNumberPosition);

		int fourthNumberPosition = Random.Range(0,6);
		int fourtNumber = (int)digitsArray[thirdNumberPosition];
		digitsArray.RemoveAt(fourthNumberPosition);

		Debug.Log("Computer number:: "+ firstNumber.ToString() + " " + secondNumber.ToString() + " " +thirdNumber.ToString());

		this.computerNumber = new Number();
		this.computerNumber.FirstNumber = firstNumber;
		this.computerNumber.SecondNumber = secondNumber;
		this.computerNumber.ThirdNumber = thirdNumber;
		this.computerNumber.FourthNumber = fourtNumber;

		return;
	}

	public Number analyzeMyGuessNumber3(Number myNumber, Number estimatedNumber){
		
		// Firstnumber analyzing
		if (myNumber.FirstNumber  == estimatedNumber.FirstNumber){
			myNumber.PozitiveResult += 1;
		}else if(myNumber.SecondNumber == estimatedNumber.FirstNumber){
			myNumber.NegativeResult += 1;
		}else if(myNumber.ThirdNumber == estimatedNumber.FirstNumber){
			myNumber.NegativeResult += 1;
		}
		
		// Second number analyzing
		if (myNumber.SecondNumber == estimatedNumber.SecondNumber){
			myNumber.PozitiveResult += 1;
		}else if(myNumber.FirstNumber == estimatedNumber.SecondNumber){
			myNumber.NegativeResult += 1;
		}else if(myNumber.ThirdNumber == estimatedNumber.SecondNumber){
			myNumber.NegativeResult += 1;
		}
		
		// Third number analyzing
		if (myNumber.ThirdNumber == estimatedNumber.ThirdNumber){
			myNumber.PozitiveResult += 1;
		}else if(myNumber.SecondNumber == estimatedNumber.ThirdNumber){
			myNumber.NegativeResult += 1;
		}else if(myNumber.FirstNumber == estimatedNumber.ThirdNumber){
			myNumber.NegativeResult += 1;
		}
		
		return myNumber;
		
	}

	public Number analyzeMyGuessNumber4(Number myNumber, Number estimatedNumber){
		
		// Firstnumber analyzing
		if (myNumber.FirstNumber  == estimatedNumber.FirstNumber){
			myNumber.PozitiveResult += 1;
		}else if(myNumber.SecondNumber == estimatedNumber.FirstNumber){
			myNumber.NegativeResult += 1;
		}else if(myNumber.ThirdNumber == estimatedNumber.FirstNumber){
			myNumber.NegativeResult += 1;
		}else if(myNumber.FourthNumber == estimatedNumber.FirstNumber){
			myNumber.NegativeResult += 1;
		}
		
		// Second number analyzing
		if (myNumber.SecondNumber == estimatedNumber.SecondNumber){
			myNumber.PozitiveResult += 1;
		}else if(myNumber.FirstNumber == estimatedNumber.SecondNumber){
			myNumber.NegativeResult += 1;
		}else if(myNumber.ThirdNumber == estimatedNumber.SecondNumber){
			myNumber.NegativeResult += 1;
		}else if(myNumber.FourthNumber == estimatedNumber.SecondNumber){
			myNumber.NegativeResult += 1;
		}
		
		// Third number analyzing
		if (myNumber.ThirdNumber == estimatedNumber.ThirdNumber){
			myNumber.PozitiveResult += 1;
		}else if(myNumber.SecondNumber == estimatedNumber.ThirdNumber){
			myNumber.NegativeResult += 1;
		}else if(myNumber.FirstNumber == estimatedNumber.ThirdNumber){
			myNumber.NegativeResult += 1;
		}else if(myNumber.FourthNumber == estimatedNumber.ThirdNumber){
			myNumber.NegativeResult += 1;
		}

		// Fourth number analyzing
		if (myNumber.FourthNumber == estimatedNumber.FourthNumber){
			myNumber.PozitiveResult += 1;
		}else if(myNumber.SecondNumber == estimatedNumber.FourthNumber){
			myNumber.NegativeResult += 1;
		}else if(myNumber.FirstNumber == estimatedNumber.FourthNumber){
			myNumber.NegativeResult += 1;
		}else if(myNumber.ThirdNumber == estimatedNumber.FourthNumber){
			myNumber.NegativeResult += 1;
		}
		
		return myNumber;
		
	}
	
	
	public Number analyzeMyNumber3(Number myNumber, Number estimatedNumber){
		
		// Firstnumber analyzing
		if (myNumber.FirstNumber  == estimatedNumber.FirstNumber){
			estimatedNumber.PozitiveResult += 1;
		}else if(myNumber.SecondNumber == estimatedNumber.FirstNumber){
			estimatedNumber.NegativeResult += 1;
		}else if(myNumber.ThirdNumber == estimatedNumber.FirstNumber){
			estimatedNumber.NegativeResult += 1;
		}
		
		// Second number analyzing
		if (myNumber.SecondNumber == estimatedNumber.SecondNumber){
			estimatedNumber.PozitiveResult += 1;
		}else if(myNumber.FirstNumber == estimatedNumber.SecondNumber){
			estimatedNumber.NegativeResult += 1;
		}else if(myNumber.ThirdNumber == estimatedNumber.SecondNumber){
			estimatedNumber.NegativeResult += 1;
		}
		
		// Third number analyzing
		if (myNumber.ThirdNumber == estimatedNumber.ThirdNumber){
			estimatedNumber.PozitiveResult += 1;
		}else if(myNumber.SecondNumber == estimatedNumber.ThirdNumber){
			estimatedNumber.NegativeResult += 1;
		}else if(myNumber.FirstNumber == estimatedNumber.ThirdNumber){
			estimatedNumber.NegativeResult += 1;
		}
		
		return estimatedNumber;
		
	}

	public Number analyzeMyNumber4(Number myNumber, Number estimatedNumber){
		
		// Firstnumber analyzing
		if (myNumber.FirstNumber  == estimatedNumber.FirstNumber){
			estimatedNumber.PozitiveResult += 1;
		}else if(myNumber.SecondNumber == estimatedNumber.FirstNumber){
			estimatedNumber.NegativeResult += 1;
		}else if(myNumber.ThirdNumber == estimatedNumber.FirstNumber){
			estimatedNumber.NegativeResult += 1;
		}else if(myNumber.FourthNumber == estimatedNumber.FirstNumber){
			estimatedNumber.NegativeResult += 1;
		}
		
		// Second number analyzing
		if (myNumber.SecondNumber == estimatedNumber.SecondNumber){
			estimatedNumber.PozitiveResult += 1;
		}else if(myNumber.FirstNumber == estimatedNumber.SecondNumber){
			estimatedNumber.NegativeResult += 1;
		}else if(myNumber.ThirdNumber == estimatedNumber.SecondNumber){
			estimatedNumber.NegativeResult += 1;
		}else if(myNumber.FourthNumber == estimatedNumber.SecondNumber){
			estimatedNumber.NegativeResult += 1;
		}
		
		// Third number analyzing
		if (myNumber.ThirdNumber == estimatedNumber.ThirdNumber){
			estimatedNumber.PozitiveResult += 1;
		}else if(myNumber.SecondNumber == estimatedNumber.ThirdNumber){
			estimatedNumber.NegativeResult += 1;
		}else if(myNumber.FirstNumber == estimatedNumber.ThirdNumber){
			estimatedNumber.NegativeResult += 1;
		}else if(myNumber.FourthNumber == estimatedNumber.ThirdNumber){
			estimatedNumber.NegativeResult += 1;
		}

		// Fourth number analyzing
		if (myNumber.FourthNumber == estimatedNumber.FourthNumber){
			estimatedNumber.PozitiveResult += 1;
		}else if(myNumber.SecondNumber == estimatedNumber.FourthNumber){
			estimatedNumber.NegativeResult += 1;
		}else if(myNumber.FirstNumber == estimatedNumber.FourthNumber){
			estimatedNumber.NegativeResult += 1;
		}else if(myNumber.ThirdNumber == estimatedNumber.FourthNumber){
			estimatedNumber.NegativeResult += 1;
		}
		
		return estimatedNumber;
		
	}

	public void createNumbersPool3(){
		ArrayList digitsArray = new ArrayList();
		digitsArray.Add (0);
		digitsArray.Add (1);
		digitsArray.Add (2);
		digitsArray.Add (3);
		digitsArray.Add (4);
		digitsArray.Add (5);
		digitsArray.Add (6);
		digitsArray.Add (7);
		digitsArray.Add (8);
		digitsArray.Add (9);
			
		numbersPool = new List<Number>();
		Number temp = null;
		int number;
		int index = 0;

		for (int i = 0; i < digitsArray.Count; i++) {
			// ilk sayiyi al sabit tut digerlerini degistir
			number = (int)doubleDigits[i];
			temp = new Number();
			temp.FirstNumber = number;
			
			for (int j = 0; j<digitsArray.Count; j++) {
				
				if(j==i)
					continue;
				
				number = (int)doubleDigits[j];
				temp.SecondNumber = number;
				
				for (int k = 0; k < digitsArray.Count; k++) {
					
					if (k==j || k==i) {
						continue;
					}
					
					number = (int)doubleDigits[k];
					temp.ThirdNumber = number;
					
					
					Number tempToAdd = new Number();
					tempToAdd.FirstNumber = temp.FirstNumber;
					tempToAdd.SecondNumber = temp.SecondNumber;
					tempToAdd.ThirdNumber = temp.ThirdNumber;
					
					index++;
					numbersPool.Add(tempToAdd);
					
//					Debug.Log("Pool Number "+index+". "+tempToAdd.FirstNumber+ " " +tempToAdd.SecondNumber+ " "+tempToAdd.ThirdNumber);
					
				}
				
			}
		}
	}

	public void createNumbersPool4(){
		ArrayList digitsArray = new ArrayList();
		digitsArray.Add (0);
		digitsArray.Add (1);
		digitsArray.Add (2);
		digitsArray.Add (3);
		digitsArray.Add (4);
		digitsArray.Add (5);
		digitsArray.Add (6);
		digitsArray.Add (7);
		digitsArray.Add (8);
		digitsArray.Add (9);
		
		numbersPool = new List<Number>();
		Number temp = null;
		int number;
		int index = 0;
		
		for (int i = 0; i < digitsArray.Count; i++) {
			// ilk sayiyi al sabit tut digerlerini degistir
			number = (int)doubleDigits[i];
			temp = new Number();
			temp.FirstNumber = number;
			
			for (int j = 0; j<digitsArray.Count; j++) {
				
				if(j==i)
					continue;
				
				number = (int)doubleDigits[j];
				temp.SecondNumber = number;
				
				for (int k = 0; k < digitsArray.Count; k++) {
					
					if (k==j || k==i) {
						continue;
					}
					
					number = (int)doubleDigits[k];
					temp.ThirdNumber = number;
					


					for (int m = 0; m < digitsArray.Count; m++) {
						
						if (m==j || m==i || m == k) {
							continue;
						}
						
						
						number = (int)doubleDigits[m];
						temp.FourthNumber = number;
						
						Number tempToAdd = new Number();
						tempToAdd.FirstNumber = temp.FirstNumber;
						tempToAdd.SecondNumber = temp.SecondNumber;
						tempToAdd.ThirdNumber = temp.ThirdNumber;
						tempToAdd.FourthNumber = temp.FourthNumber;

						index++;
						numbersPool.Add(tempToAdd);
						
						//					Debug.Log("Pool Number "+index+". "+tempToAdd.FirstNumber+ " " +tempToAdd.SecondNumber+ " "+tempToAdd.ThirdNumber);
					}

				}
				
			}
		}
	}

	public void createMyNumbersPool3(){
		ArrayList digitsArray = new ArrayList();
		digitsArray.Add (0);
		digitsArray.Add (1);
		digitsArray.Add (2);
		digitsArray.Add (3);
		digitsArray.Add (4);
		digitsArray.Add (5);
		digitsArray.Add (6);
		digitsArray.Add (7);
		digitsArray.Add (8);
		digitsArray.Add (9);
		
		myNumbersPool = new List<Number>();
		Number temp = null;
		int number;
		int index = 0;
		
		for (int i = 0; i < digitsArray.Count; i++) {
			// ilk sayiyi al sabit tut digerlerini degistir
			number = (int)myDoubleDigits[i];
			temp = new Number();
			temp.FirstNumber = number;
			
			for (int j = 0; j<digitsArray.Count; j++) {
				
				if(j==i)
					continue;
				
				number = (int)myDoubleDigits[j];
				temp.SecondNumber = number;
				
				for (int k = 0; k < digitsArray.Count; k++) {
					
					if (k==j || k==i) {
						continue;
					}
					
					number = (int)myDoubleDigits[k];
					temp.ThirdNumber = number;
					
					
					Number tempToAdd = new Number();
					tempToAdd.FirstNumber = temp.FirstNumber;
					tempToAdd.SecondNumber = temp.SecondNumber;
					tempToAdd.ThirdNumber = temp.ThirdNumber;
					
					index++;
					myNumbersPool.Add(tempToAdd);
					
//					Debug.Log("Pool Number "+index+". "+tempToAdd.FirstNumber+ " " +tempToAdd.SecondNumber+ " "+tempToAdd.ThirdNumber);
					
				}
				
			}
		}
	}

	public void createMyNumbersPool4(){
		ArrayList digitsArray = new ArrayList();
		digitsArray.Add (0);
		digitsArray.Add (1);
		digitsArray.Add (2);
		digitsArray.Add (3);
		digitsArray.Add (4);
		digitsArray.Add (5);
		digitsArray.Add (6);
		digitsArray.Add (7);
		digitsArray.Add (8);
		digitsArray.Add (9);
		
		myNumbersPool = new List<Number>();
		Number temp = null;
		int number;
		int index = 0;
		
		for (int i = 0; i < digitsArray.Count; i++) {
			// ilk sayiyi al sabit tut digerlerini degistir
			number = (int)myDoubleDigits[i];
			temp = new Number();
			temp.FirstNumber = number;
			
			for (int j = 0; j<digitsArray.Count; j++) {
				
				if(j==i)
					continue;
				
				number = (int)myDoubleDigits[j];
				temp.SecondNumber = number;
				
				for (int k = 0; k < digitsArray.Count; k++) {
					
					if (k==j || k==i) {
						continue;
					}
					
					number = (int)myDoubleDigits[k];
					temp.ThirdNumber = number;
					

					for (int m = 0; m < digitsArray.Count; m++) {
						
						if (m==j || m==i || m == k) {
							continue;
						}
						
						
						number = (int)myDoubleDigits[m];
						temp.FourthNumber = number;
						
						Number tempToAdd = new Number();
						tempToAdd.FirstNumber = temp.FirstNumber;
						tempToAdd.SecondNumber = temp.SecondNumber;
						tempToAdd.ThirdNumber = temp.ThirdNumber;
						
						index++;
						myNumbersPool.Add(tempToAdd);
						
						//					Debug.Log("Pool Number "+index+". "+tempToAdd.FirstNumber+ " " +tempToAdd.SecondNumber+ " "+tempToAdd.ThirdNumber);
					}


										
				}
				
			}
		}
	}

	public void createPossibleNumbersPool3(Number myNumberGuess){
		
		List<Number> tempNumbersPool = new List<Number>();
		
		Number myGuessNumber = new Number();
		myGuessNumber.FirstNumber = myNumberGuess.FirstNumber;
		myGuessNumber.SecondNumber = myNumberGuess.SecondNumber;
		myGuessNumber.ThirdNumber = myNumberGuess.ThirdNumber;
		myGuessNumber.PozitiveResult = myNumberGuess.PozitiveResult;
		myGuessNumber.NegativeResult = myNumberGuess.NegativeResult;

		Number checkNumber = null;
		int index = 0;
		Number resultNumber = null;
		for (int i = 0; i<numbersPool.Count; i++) {
			index++;
			checkNumber = (Number)numbersPool[i];
			checkNumber.PozitiveResult = 0;
			checkNumber.NegativeResult = 0;
			
			resultNumber = analyzeMyGuessNumber3(checkNumber, myGuessNumber);
			
			if(resultNumber.PozitiveResult ==  myGuessNumber.PozitiveResult && resultNumber.NegativeResult ==  myGuessNumber.NegativeResult){
				tempNumbersPool.Add(numbersPool[i]);
//				Debug.Log("Guncel Pool Number "+index.ToString()+". "+ numbersPool[i].FirstNumber.ToString()+ " " +numbersPool[i].SecondNumber.ToString()+ " "+numbersPool[i].ThirdNumber.ToString());
			}
			
		}
		
		numbersPool.Clear();
		numbersPool.AddRange(tempNumbersPool);
//		Debug.Log("Guncel muhtemel sayi havuzu:"+ numbersPool.Count.ToString());
		
	}

	public void createPossibleNumbersPool4(Number myNumberGuess){
		
		List<Number> tempNumbersPool = new List<Number>();
		
		Number myGuessNumber = new Number();
		myGuessNumber.FirstNumber = myNumberGuess.FirstNumber;
		myGuessNumber.SecondNumber = myNumberGuess.SecondNumber;
		myGuessNumber.ThirdNumber = myNumberGuess.ThirdNumber;
		myGuessNumber.FourthNumber = myNumberGuess.FourthNumber;

		myGuessNumber.PozitiveResult = myNumberGuess.PozitiveResult;
		myGuessNumber.NegativeResult = myNumberGuess.NegativeResult;
		
		Number checkNumber = null;
		int index = 0;
		Number resultNumber = null;
		for (int i = 0; i<numbersPool.Count; i++) {
			index++;
			checkNumber = (Number)numbersPool[i];
			checkNumber.PozitiveResult = 0;
			checkNumber.NegativeResult = 0;
			
			resultNumber = analyzeMyGuessNumber4(checkNumber, myGuessNumber);
			
			if(resultNumber.PozitiveResult ==  myGuessNumber.PozitiveResult && resultNumber.NegativeResult ==  myGuessNumber.NegativeResult){
				tempNumbersPool.Add(numbersPool[i]);
				//				Debug.Log("Guncel Pool Number "+index.ToString()+". "+ numbersPool[i].FirstNumber.ToString()+ " " +numbersPool[i].SecondNumber.ToString()+ " "+numbersPool[i].ThirdNumber.ToString());
			}
			
		}
		
		numbersPool.Clear();
		numbersPool.AddRange(tempNumbersPool);
		//		Debug.Log("Guncel muhtemel sayi havuzu:"+ numbersPool.Count.ToString());
		
	}

	public void createMyPossibleNumbersPool3(Number myNumberGuess){
		
		List<Number> tempNumbersPool = new List<Number>();
		
		Number myGuessNumber = new Number();
		myGuessNumber.FirstNumber = myNumberGuess.FirstNumber;
		myGuessNumber.SecondNumber = myNumberGuess.SecondNumber;
		myGuessNumber.ThirdNumber = myNumberGuess.ThirdNumber;
		myGuessNumber.PozitiveResult = myNumberGuess.PozitiveResult;
		myGuessNumber.NegativeResult = myNumberGuess.NegativeResult;
		
		Number checkNumber = null;
		int index = 0;
		Number resultNumber = null;
		for (int i = 0; i<myNumbersPool.Count; i++) {
			index++;
			checkNumber = (Number)myNumbersPool[i];
			checkNumber.PozitiveResult = 0;
			checkNumber.NegativeResult = 0;
			
			resultNumber = analyzeMyGuessNumber3(checkNumber, myGuessNumber);
			
			if(resultNumber.PozitiveResult ==  myGuessNumber.PozitiveResult && resultNumber.NegativeResult ==  myGuessNumber.NegativeResult){
				tempNumbersPool.Add(myNumbersPool[i]);
//				Debug.Log("My Guncel Pool Number "+index.ToString()+". "+ myNumbersPool[i].FirstNumber.ToString()+ " " +myNumbersPool[i].SecondNumber.ToString()+ " "+myNumbersPool[i].ThirdNumber.ToString());
			}
			
		}
		
		myNumbersPool.Clear();
		myNumbersPool.AddRange(tempNumbersPool);
		Debug.Log("My Guncel muhtemel sayi havuzu:"+ myNumbersPool.Count.ToString());
		
	}

	public void createMyPossibleNumbersPool4(Number myNumberGuess){
		
		List<Number> tempNumbersPool = new List<Number>();
		
		Number myGuessNumber = new Number();
		myGuessNumber.FirstNumber = myNumberGuess.FirstNumber;
		myGuessNumber.SecondNumber = myNumberGuess.SecondNumber;
		myGuessNumber.ThirdNumber = myNumberGuess.ThirdNumber;
		myGuessNumber.FourthNumber = myNumberGuess.FourthNumber;

		myGuessNumber.PozitiveResult = myNumberGuess.PozitiveResult;
		myGuessNumber.NegativeResult = myNumberGuess.NegativeResult;
		
		Number checkNumber = null;
		int index = 0;
		Number resultNumber = null;
		for (int i = 0; i<myNumbersPool.Count; i++) {
			index++;
			checkNumber = (Number)myNumbersPool[i];
			checkNumber.PozitiveResult = 0;
			checkNumber.NegativeResult = 0;
			
			resultNumber = analyzeMyGuessNumber4(checkNumber, myGuessNumber);
			
			if(resultNumber.PozitiveResult ==  myGuessNumber.PozitiveResult && resultNumber.NegativeResult ==  myGuessNumber.NegativeResult){
				tempNumbersPool.Add(myNumbersPool[i]);
				//				Debug.Log("My Guncel Pool Number "+index.ToString()+". "+ myNumbersPool[i].FirstNumber.ToString()+ " " +myNumbersPool[i].SecondNumber.ToString()+ " "+myNumbersPool[i].ThirdNumber.ToString());
			}
			
		}
		
		myNumbersPool.Clear();
		myNumbersPool.AddRange(tempNumbersPool);
		Debug.Log("My Guncel muhtemel sayi havuzu:"+ myNumbersPool.Count.ToString());
		
	}

	public Number caseFourMinusOne(List<Number> guessNumberHistory) {
		Number myNumberGuess = null;
		// tutan rakamlarin yerini bul ve olmayan sayilardan havuzda olmayan bir tahmin yap
		
		
		int ilk = 0;
		int ikinci = 0;
		int ucuncu = 0;
		
		int first = 0;
		int second = 0;
		int third = 0;
		
		for (int i = 0; i < numbersPool.Count; i++) {
			Number temp2 = new Number();
			temp2 = numbersPool[i];
			
			if (first != 0 && (first == temp2.FirstNumber)) {
				ilk++;
				
			}
			
			if(second != 0 && (second == temp2.SecondNumber)){
				// ikinci siradaki sayilar yok
				ikinci ++;
				
			}
			
			if(third != 0 && (third == temp2.ThirdNumber)){
				// ucuncu siradaki sayilar yok
				ucuncu++;
			}
			
			first = temp2.FirstNumber;
			second = temp2.SecondNumber;
			third = temp2.ThirdNumber;
			
		}
		
		if ((ilk == numbersPool.Count-1) &&
		    (ikinci == numbersPool.Count-1)) {
			//1 ve 2 ayni
			
			ArrayList ucuncuSira = new ArrayList();
			for (int j = 0; j< numbersPool.Count; j++) {
				
				Number temp3 = new Number();
				temp3 = numbersPool[j];
				
				ucuncuSira.Add(temp3.ThirdNumber);
			}


			int firstNumberPosition = Random.Range(0,(ucuncuSira.Count-1));
			int firstNumber = (int)ucuncuSira[firstNumberPosition];
			ucuncuSira.RemoveAt(firstNumberPosition);

			int secondNumberPosition = Random.Range(0,(ucuncuSira.Count-1));
			int secondNumber = (int)ucuncuSira[secondNumberPosition];
			ucuncuSira.RemoveAt(secondNumberPosition);

			int thirdNumberPosition = Random.Range(0,(ucuncuSira.Count-1));
			int thirdNumber = (int)ucuncuSira[thirdNumberPosition];
			ucuncuSira.RemoveAt(thirdNumberPosition);

			myNumberGuess = new Number();
			myNumberGuess.FirstNumber = firstNumber;
			myNumberGuess.SecondNumber = secondNumber;
			myNumberGuess.ThirdNumber = thirdNumber;


			Debug.Log((guessNumberHistory.Count+1).ToString() + "lu Tahmin: "+ myNumberGuess.FirstNumber.ToString() + " " + myNumberGuess.SecondNumber.ToString() + " " +myNumberGuess.ThirdNumber.ToString());


			return myNumberGuess;
			
		}else if((ilk == numbersPool.Count-1) &&
		         (ucuncu == numbersPool.Count-1)){
			//1 ve 3 ayni
			
			ArrayList ikinciSira = new ArrayList();
			for (int j = 0; j< numbersPool.Count; j++) {
				
				Number temp3 = new Number();
				temp3 = numbersPool[j];
				
				ikinciSira.Add(temp3.ThirdNumber);
			}


			int firstNumberPosition = Random.Range(0,(ikinciSira.Count-1));
			int firstNumber = (int)ikinciSira[firstNumberPosition];
			ikinciSira.RemoveAt(firstNumberPosition);
			
			int secondNumberPosition = Random.Range(0,(ikinciSira.Count-1));
			int secondNumber = (int)ikinciSira[secondNumberPosition];
			ikinciSira.RemoveAt(secondNumberPosition);
			
			int thirdNumberPosition = Random.Range(0,(ikinciSira.Count-1));
			int thirdNumber = (int)ikinciSira[thirdNumberPosition];
			ikinciSira.RemoveAt(thirdNumberPosition);
			
			myNumberGuess = new Number();
			myNumberGuess.FirstNumber = firstNumber;
			myNumberGuess.SecondNumber = secondNumber;
			myNumberGuess.ThirdNumber = thirdNumber;

			Debug.Log((guessNumberHistory.Count+1).ToString() + "lu Tahmin: "+ myNumberGuess.FirstNumber.ToString() + " " + myNumberGuess.SecondNumber.ToString() + " " +myNumberGuess.ThirdNumber.ToString());

			
			return myNumberGuess;
			
		}else if((ikinci == numbersPool.Count-1) &&
		         (ucuncu == numbersPool.Count-1)){
			//2 ve 3 ayni
			ArrayList ilkSira = new ArrayList();
			for (int j = 0; j< numbersPool.Count; j++) {

				Number temp3 = new Number();
				temp3 = numbersPool[j];
				
				ilkSira.Add(temp3.ThirdNumber);

			}

			int firstNumberPosition = Random.Range(0,(ilkSira.Count-1));
			int firstNumber = (int)ilkSira[firstNumberPosition];
			ilkSira.RemoveAt(firstNumberPosition);
			
			int secondNumberPosition = Random.Range(0,(ilkSira.Count-1));
			int secondNumber = (int)ilkSira[secondNumberPosition];
			ilkSira.RemoveAt(secondNumberPosition);
			
			int thirdNumberPosition = Random.Range(0,(ilkSira.Count-1));
			int thirdNumber = (int)ilkSira[thirdNumberPosition];
			ilkSira.RemoveAt(thirdNumberPosition);
			
			myNumberGuess = new Number();
			myNumberGuess.FirstNumber = firstNumber;
			myNumberGuess.SecondNumber = secondNumber;
			myNumberGuess.ThirdNumber = thirdNumber;
			
			Debug.Log((guessNumberHistory.Count+1).ToString() + "lu Tahmin: "+ myNumberGuess.FirstNumber.ToString() + " " + myNumberGuess.SecondNumber.ToString() + " " +myNumberGuess.ThirdNumber.ToString());

			return myNumberGuess;
		}
		
		
		return myNumberGuess;
	}

	public Number createNumberByHardAI3(List<Number> guessNumberHistory) {
		
		Number myNumberGuess = new Number();
		
		if (guessNumberHistory != null && guessNumberHistory.Count > 0) {
			Number temp = (Number)guessNumberHistory[guessNumberHistory.Count-1];
			if (temp.PozitiveResult == 4 && temp.NegativeResult == -1 && guessNumberHistory.Count > 0 && numbersPool.Count >= 3) {
				
				myNumberGuess = this.caseFourMinusOne(guessNumberHistory);
				if (myNumberGuess != null) {
					return myNumberGuess;
				}
			}
		}
		
		int numberPosition = Random.Range(0,(numbersPool.Count-1));
		myNumberGuess = numbersPool[numberPosition];
		numbersPool.RemoveAt(numberPosition);

		if(guessNumberHistory != null)
			Debug.Log((guessNumberHistory.Count+1).ToString() + "lu Tahmin: "+ myNumberGuess.FirstNumber.ToString() + " " + myNumberGuess.SecondNumber.ToString() + " " +myNumberGuess.ThirdNumber.ToString());

		return myNumberGuess;
	}

	public Number createNumberByHardAI4(List<Number> guessNumberHistory) {
		
		Number myNumberGuess = new Number();
		
		int numberPosition = Random.Range(0,(numbersPool.Count-1));
		myNumberGuess = numbersPool[numberPosition];
		numbersPool.RemoveAt(numberPosition);
		
		if(guessNumberHistory != null)
			Debug.Log((guessNumberHistory.Count+1).ToString() + "lu Tahmin: "+ myNumberGuess.FirstNumber.ToString() + " " + myNumberGuess.SecondNumber.ToString() + " " +myNumberGuess.ThirdNumber.ToString());
		
		return myNumberGuess;
	}


	private bool isFindAllNumbersInOneGuess(Number estimatedNumber) {
		
		bool resultValue = false;

		int total = estimatedNumber.PozitiveResult + estimatedNumber.NegativeResult;
		int condition = 3;

		//Eger pozitif deger 3 den buyuk ve negatif 0 ise sayilar bulunmustur.
		if(total >= condition){
			numberFounded = estimatedNumber;
			tumSayilarBirTahmindeBulundu = true;
			foundedDigits.Clear();
			foundedDigits.Add(numberFounded.FirstNumber);
			foundedDigits.Add(numberFounded.SecondNumber);
			foundedDigits.Add(numberFounded.ThirdNumber);

			resultValue = true;
		}
		
		return resultValue;
	}

	public Number createNumberByVeryHardAI3(List<Number> guessNumberHistory) {
		
		Number myNumberGuess = new Number();
		
		// 1. TAHMIN
		if (guessNumberHistory.Count == 0) {
			// rastgele 3 sayi cekilecek ve mevcut sayi listesinden cikarilacak

			int firstNumberPosition = Random.Range(0,(doubleDigits.Count-1));
			int firstNumber = (int)doubleDigits[firstNumberPosition];
			doubleDigits.RemoveAt(firstNumberPosition);

			int secondNumberPosition = Random.Range(0,(doubleDigits.Count-1));
			int secondNumber = (int)doubleDigits[secondNumberPosition];
			doubleDigits.RemoveAt(secondNumberPosition);

			int thirdNumberPosition = Random.Range(0,(doubleDigits.Count-1));
			int thirdNumber = (int)doubleDigits[thirdNumberPosition];
			doubleDigits.RemoveAt(thirdNumberPosition);

			Debug.Log("1.Tahmin:: "+ firstNumber.ToString() + " " + secondNumber.ToString() + " " +thirdNumber.ToString());

			myNumberGuess.FirstNumber = firstNumber;
			myNumberGuess.SecondNumber = secondNumber;
			myNumberGuess.ThirdNumber = thirdNumber;
			
			return myNumberGuess;
		}
		
		// 2. TAHMIN
		if (guessNumberHistory.Count == 1) {
			
			// ilk tahmin sonucunda tum sayilar bulundumu?
			if (isFindAllNumbersInOneGuess(guessNumberHistory[guessNumberHistory.Count-1])) {
				//tum sayilar tek tahmin icinde bulundu..
				Debug.Log(@"tum sayilar tek tahmin icinde bulundu..");
				tumSayilarBirTahmindeBulundu = true;
				myNumberGuess = createNumberByHardAI3(null);
				if (myNumberGuess != null) {
					return myNumberGuess;
				}
			}else{
				// tum sayilar bulunamadi.
				// olmayan numaralari listeye ekle
				Number temp = (Number)guessNumberHistory[guessNumberHistory.Count-1];
				if (temp.NegativeResult == 0 && temp.PozitiveResult == 0){
					notAvailableDigits.Add(temp.FirstNumber);
					notAvailableDigits.Add(temp.SecondNumber);
					notAvailableDigits.Add(temp.ThirdNumber);
				}else{
					availableDigits.Add(temp.FirstNumber);
					availableDigits.Add(temp.SecondNumber);
					availableDigits.Add(temp.ThirdNumber);
				}
				
				// rastgele 3 sayi cekilecek ve mevcut sayi listesinden cikarilacak
				int firstNumberPosition = Random.Range(0,(doubleDigits.Count-1));
				int firstNumber = (int)doubleDigits[firstNumberPosition];
				doubleDigits.RemoveAt(firstNumberPosition);
				
				int secondNumberPosition = Random.Range(0,(doubleDigits.Count-1));
				int secondNumber = (int)doubleDigits[secondNumberPosition];
				doubleDigits.RemoveAt(secondNumberPosition);
				
				int thirdNumberPosition = Random.Range(0,(doubleDigits.Count-1));
				int thirdNumber =(int) doubleDigits[thirdNumberPosition];
				doubleDigits.RemoveAt(thirdNumberPosition);
				
				Debug.Log("2.Tahmin:: "+ firstNumber.ToString() + " " + secondNumber.ToString() + " " +thirdNumber.ToString());

				myNumberGuess.FirstNumber = firstNumber;
				myNumberGuess.SecondNumber = secondNumber;
				myNumberGuess.ThirdNumber = thirdNumber;
				
				return myNumberGuess;
				
			}
			
		}


		return createNumberByHardAI3(null);
	}


	public Number createNumberByVeryHardAI4(List<Number> guessNumberHistory) {
		
		Number myNumberGuess = new Number();
		
		// 1. TAHMIN
		if (guessNumberHistory.Count == 0) {
			// rastgele 3 sayi cekilecek ve mevcut sayi listesinden cikarilacak
			
			int firstNumberPosition = Random.Range(0,(doubleDigits.Count-1));
			int firstNumber = (int)doubleDigits[firstNumberPosition];
			doubleDigits.RemoveAt(firstNumberPosition);
			
			int secondNumberPosition = Random.Range(0,(doubleDigits.Count-1));
			int secondNumber = (int)doubleDigits[secondNumberPosition];
			doubleDigits.RemoveAt(secondNumberPosition);
			
			int thirdNumberPosition = Random.Range(0,(doubleDigits.Count-1));
			int thirdNumber = (int)doubleDigits[thirdNumberPosition];
			doubleDigits.RemoveAt(thirdNumberPosition);

			int fourthNumberPosition = Random.Range(0,(doubleDigits.Count-1));
			int fourthNumber = (int)doubleDigits[fourthNumberPosition];
			doubleDigits.RemoveAt(fourthNumberPosition);
			
			Debug.Log("1.Tahmin:: "+ firstNumber.ToString() + " " + secondNumber.ToString() + " " +thirdNumber.ToString());
			
			myNumberGuess.FirstNumber = firstNumber;
			myNumberGuess.SecondNumber = secondNumber;
			myNumberGuess.ThirdNumber = thirdNumber;
			myNumberGuess.FourthNumber = fourthNumber;
			
			return myNumberGuess;
		}
		
		// 2. TAHMIN
		if (guessNumberHistory.Count == 1) {
			
			// ilk tahmin sonucunda tum sayilar bulundumu?
			if (isFindAllNumbersInOneGuess(guessNumberHistory[guessNumberHistory.Count-1])) {
				//tum sayilar tek tahmin icinde bulundu..
				Debug.Log(@"tum sayilar tek tahmin icinde bulundu..");
				tumSayilarBirTahmindeBulundu = true;
				myNumberGuess = createNumberByHardAI4(null);
				if (myNumberGuess != null) {
					return myNumberGuess;
				}
			}else{
				// tum sayilar bulunamadi.
				// olmayan numaralari listeye ekle
				Number temp = (Number)guessNumberHistory[guessNumberHistory.Count-1];
				int total = temp.PozitiveResult + temp.NegativeResult;
				if (total == 0){
					notAvailableDigits.Add(temp.FirstNumber);
					notAvailableDigits.Add(temp.SecondNumber);
					notAvailableDigits.Add(temp.ThirdNumber);
				}else{
					availableDigits.Add(temp.FirstNumber);
					availableDigits.Add(temp.SecondNumber);
					availableDigits.Add(temp.ThirdNumber);
				}
				
				// rastgele 3 sayi cekilecek ve mevcut sayi listesinden cikarilacak
				int firstNumberPosition = Random.Range(0,(doubleDigits.Count-1));
				int firstNumber = (int)doubleDigits[firstNumberPosition];
				doubleDigits.RemoveAt(firstNumberPosition);
				
				int secondNumberPosition = Random.Range(0,(doubleDigits.Count-1));
				int secondNumber = (int)doubleDigits[secondNumberPosition];
				doubleDigits.RemoveAt(secondNumberPosition);
				
				int thirdNumberPosition = Random.Range(0,(doubleDigits.Count-1));
				int thirdNumber =(int) doubleDigits[thirdNumberPosition];
				doubleDigits.RemoveAt(thirdNumberPosition);

				int fourthNumberPosition = Random.Range(0,(doubleDigits.Count-1));
				int fourthNumber =(int) doubleDigits[fourthNumberPosition];
				doubleDigits.RemoveAt(thirdNumberPosition);
				
				Debug.Log("2.Tahmin:: "+ firstNumber.ToString() + " " + secondNumber.ToString() + " " +thirdNumber.ToString()+ " " +fourthNumber.ToString());
				
				myNumberGuess.FirstNumber = firstNumber;
				myNumberGuess.SecondNumber = secondNumber;
				myNumberGuess.ThirdNumber = thirdNumber;
				myNumberGuess.FourthNumber = fourthNumber;
				
				return myNumberGuess;
				
			}
			
		}
		
		
		return createNumberByHardAI4(null);
	}


}
