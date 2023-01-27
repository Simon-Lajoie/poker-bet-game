using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ==============================================================================================================================================================================
// Simon Lajoie
// ==============================================================================================================================================================================
// POKER BETTING GAME FEATURES
// ==============================================================================================================================================================================
// - Simple console menu with 3 options (Play, Rules and Test)
// - Random cards are generated every turn
// - All winning poker hands are checked (Pair, Two Pair, Three of a Kind, Straight, Flush, Full House, Four of a Kind, Straight Flush, Royal Flush)
// - Different earnings based on winning poker hand
// - Player loses the game when he has no more money to bet
// ==============================================================================================================================================================================  
namespace PokerBetGame
{
    class Program
    {
        // =====================================================================================         
        // global variables         
        // =====================================================================================   
        const int INIT_AMT_BANKROLL = 1000, MAX_DISCARD_AMT = 4, NUM_CARDS = 52, HAND_SIZE = 5;
        const int PAIR_PAY = 1, TWO_PAIR_PAY = 2, THREE_OF_KIND_PAY = 3, STRAIGHT_PAY = 4, FLUSH_PAY = 6, FULL_HOUSE_PAY = 9, FOUR_OF_KIND_PAY = 25, STRAIGHT_FLUSH_PAY = 50, ROYAL_FLUSH_PAY = 250;
        static void Main(string[] args)
        {
            MainMenu();
        }
        // =====================================================================================         
        // Main Menu       
        // ===================================================================================== 
        static void MainMenu()
        {
            int bankroll = INIT_AMT_BANKROLL, amountWon = 0, bet = 0, amountToMultiply, choice;
            bool gameOver = false;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("=============== Poker Game ===============");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("1. Play \n2. Rules \n3. Winning Hands & Payout \n4. Test \n5. Exit");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("==========================================");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Please enter your desired option: ");
            Console.ResetColor();
            string input = Console.ReadLine();
            // User input verification
            while (!int.TryParse(input, out choice) || int.Parse(input) > 5 || int.Parse(input) < 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Error, please enter a valid menu choice: ");
                Console.ResetColor();
                input = Console.ReadLine();
            }
            // =====================================================================================         
            // Start the game        
            // ===================================================================================== 
            if (choice == 1)
            {
                Console.Clear();
                // Get the bet amount and display the bank roll
                AskBetAndDisplayBankroll(ref bankroll, ref bet);
                Deck deck = new Deck();
                Card[] hand = new Card[HAND_SIZE];

                // Play the game until game over (no money left or user exit)
                while (!gameOver)
                {
                    FillHand(hand, deck);    //
                    SortHand(hand);          //
                    DisplayHand(hand);       //
                    DiscardCard(deck, hand);
                    SortHand(hand);
                    DisplayHand(hand);
                    amountToMultiply = CheckWinCondition(hand);
                    WinOrLose(amountToMultiply, ref bankroll, amountWon, bet, ref gameOver);
                    if (!gameOver)
                    {
                        AskBetAndDisplayBankroll(ref bankroll, ref bet);
                    }
                }
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Press enter to exit...");
                Console.ResetColor();
                Console.ReadLine();
            }
            // Show rules
            else if (choice == 2)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("=============== RULES ===============");
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("Goal: Try to get winning poker hands and make as much money as you can.");
                Console.WriteLine("End: You lose when you have 0$ left in your bank or when you exit.");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("------------ HOW TO PLAY ------------");
                Console.ResetColor();
                Console.WriteLine("1. You start with 1000$ in your bank.");
                Console.WriteLine("2. Place a bet ranging from 1$ to 1000$.");
                Console.WriteLine("3. 5 cards will be randomly generated.");
                Console.WriteLine("You can now choose to replace any of them. You can only replace a specific card once.");
                Console.WriteLine("Your goal is to get winning poker hands. The better the hand, the bigger the multiplier will be applied to your bet.");
                Console.WriteLine("5. Once you are done replacing cards, if any, the hand will be updated.");
                Console.WriteLine("You will see a winning or losing message along with your gains and money left.");
                Console.WriteLine("6. You can choose to play again or leave and keep your earnings.");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("======================================");
                Console.ResetColor();
                Console.WriteLine("Press any key to return to main menu...");
                Console.ReadKey();
                Console.Clear();
                MainMenu();
            }
            // Show winning hands
            else if (choice == 3)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("=============== Winning Hands & Payout ===============");
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("1. Pair bigger than 10 | Payout: x1.0");
                Console.WriteLine("2. Two Pair            | Payout: x2.0");
                Console.WriteLine("3. Three of a kind     | Payout: x3.0");
                Console.WriteLine("4. Straight            | Payout: x4.0");
                Console.WriteLine("5. Flush               | Payout: x6.0");
                Console.WriteLine("6. Full house          | Payout: x9.0");
                Console.WriteLine("7. Four of a kind      | Payout: x25.0");
                Console.WriteLine("8. Straight flush      | Payout: x50.0");
                Console.WriteLine("9. Royal flush         | Payout: x250.0");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("======================================================");
                Console.ResetColor();
                Console.WriteLine("Press any key to return to main menu...");
                Console.ReadKey();
                Console.Clear();
                MainMenu();
            }
            // Test
            else if (choice == 4)
            {
                Console.Clear();
                while (choice != 0)
                {
                    choice = TestingWinConditions(choice);
                }
                Console.Clear();
                MainMenu();
            }
            else
            {
                System.Environment.Exit(0);
            }
        }
        // =====================================================================================         
        // Bank and bet methods 
        // ===================================================================================== 
        // Ask the user for a bet and display bankroll
        static void AskBetAndDisplayBankroll(ref int bankroll, ref int bet)
        {
            bet = AskForBet(bankroll);
            bankroll = SubtractTheBet(bankroll, bet);
            DisplayBankroll(bankroll);
            DisplayBet(bet);
            Console.WriteLine();
        }
        // Ask the user for a bet and display bankroll
        static int DisplayAndAddMoney(int amountToMultiply, int bet, ref int bankroll)
        {
            bankroll += (bet * amountToMultiply);
            return bet * amountToMultiply;
        }
        // Display bankroll
        static void DisplayBankroll(int bankroll)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Money in bank: " + bankroll.ToString("C"));
            Console.ResetColor();
        }
        // Display bet
        static void DisplayBet(int bet)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Current bet: " + bet.ToString("C"));
            Console.ResetColor();
        }
        // Subtract the bet from the bankroll
        static int SubtractTheBet(int bankroll, int bet)
        {
            return bankroll - bet;
        }
        // Ask the user for a bet
        static int AskForBet(int bankroll)
        {
            int bet;
            Console.Clear();
            DisplayBankroll(bankroll);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Enter the amount you would like to bet" + " (1.." + bankroll + "): ");
            Console.ResetColor();
            string input = Console.ReadLine();
            while (!int.TryParse(input, out bet) || int.Parse(input) < 1 || int.Parse(input) > bankroll)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid input! The amount must be between 1 and " + bankroll);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Enter the amount you would like to bet" + " (1.." + bankroll + "): ");
                Console.ResetColor();
                input = Console.ReadLine();
            }
            return bet;
        }
        // Generate random cards and fill the hand
        static void FillHand(Card[] hand, Deck deck)
        {
            deck.shuffle();
            for (int index = 0; index < HAND_SIZE; index++)
            {
                if (!deck.isEmpty())
                {
                    hand[index] = deck.dealACard();
                }
            }
        }
        // Sort the hand
        static void SortHand(Card[] hand) // Insertion Sort
        {
            int newIndex;
            Card tempCard;
            for (int index = 1; index < hand.Length; index++)
            {
                tempCard = hand[index];
                newIndex = index - 1;
                while (newIndex >= 0 && hand[newIndex].getFaceValue() > tempCard.getFaceValue())
                {
                    hand[newIndex + 1] = hand[newIndex];
                    newIndex--;
                }
                hand[newIndex + 1] = tempCard;
            }
        }
        // Display the hand
        static void DisplayHand(Card[] hand)
        {
            Console.WriteLine();
            for (int index = 0; index < hand.Length; index++)
            {
                Console.WriteLine("[" + (index + 1) + "] " + hand[index].ToString());
            }
            Console.WriteLine();
        }
        // Ask the user to discard a card, and replace the requested card with a newly generated one
        static void DiscardCard(Deck deck, Card[] hand)
        {
            int choice, numDiscard = 0, index = 0;
            // Store which cards we have replaced
            int[] replacedCards = new int[HAND_SIZE]; 
            for (int i=0; i<replacedCards.Length; i++)
            {
                // Change all default values in replacedCards array to -2. (Could be another number, but it has to be different than the default 0 since 0 is a valid input)
                replacedCards[i] = -2;
            }
            choice = getReplacementCardChoice(replacedCards);
            while (choice != -1 && numDiscard < MAX_DISCARD_AMT)
            {
                Card newCard = deck.dealACard();
                // The new generated card can't be the same as the one being replaced
                while (hand[choice] == newCard) 
                {
                    newCard = deck.dealACard();
                }
                hand[choice] = newCard;
                numDiscard++;
                replacedCards[index] = choice;
                index++;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Card successfuly replaced...");
                Console.ResetColor();
                Console.WriteLine();
                choice = getReplacementCardChoice(replacedCards);
            }
        }
        static int getReplacementCardChoice(int[] replacedCards)
        {
            bool isAlreadyReplaced;
            bool validInput;
            validInput = true;
            isAlreadyReplaced = true;
            int choice = -1;
            while ((!validInput && isAlreadyReplaced) || (validInput && isAlreadyReplaced))
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Choose the card you'd like to replace, or enter 0 to continue: ");
                Console.ResetColor();
                string input = Console.ReadLine();
                while (!int.TryParse(input, out choice) || int.Parse(input) > HAND_SIZE || int.Parse(input) < 0) // User input verification
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Error, enter a valid card or enter 0 to quit: ");
                    Console.ResetColor();
                    input = Console.ReadLine();
                }
                validInput = true;
                choice -= 1;
                isAlreadyReplaced = false;
                // check if user already replaced this card
                foreach (int chosen in replacedCards) 
                {
                    if (choice == chosen)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Error, card already chosen...");
                        Console.ResetColor();
                        Console.WriteLine();
                        isAlreadyReplaced = true;
                    }
                }
            }
            return choice;
        }
        // =====================================================================================         
        // Check all win conditions (Winning poker hands)
        // NOTE: The logic for all of my checking methods is based on the fact that the hand is sorted.
        // ===================================================================================== 
        static int CheckWinCondition(Card[] hand)
        {
            int numPair = 0, numTwoPair = 0, numStraight = 0, numFourOfKind = 0, numThreeOfKind = 0, numFlush = 0, numFullHouse = 0, numStraightFlush = 0, numRoyalFlush = 0;
            numStraight = CheckStraight(hand);
            numFlush = CheckFlush(hand);
            numFourOfKind = CheckFourOfKind(hand);
            numFullHouse = CheckFullHouse(hand);
            numThreeOfKind = CheckThreeOfKind(hand, numFourOfKind, numFullHouse);
            numTwoPair = CheckTwoPair(hand, numFourOfKind, numFullHouse, numThreeOfKind);
            numPair = CheckPair(hand, numThreeOfKind, numFourOfKind, numFullHouse, numTwoPair);
            if (numStraight == 1 && numFlush == 1)
            {
                numStraightFlush = 1;
                if (hand[0].getFaceValue() == Card.FaceValue.Ten)
                {
                    numRoyalFlush = 1;
                }
            }
            if (numRoyalFlush == 1)
            {
                return ROYAL_FLUSH_PAY;
            }
            else if (numStraightFlush == 1)
            {
                return STRAIGHT_FLUSH_PAY;
            }
            else if (numFourOfKind == 1)
            {
                return FOUR_OF_KIND_PAY;
            }
            else if (numFullHouse == 1)
            {
                return FULL_HOUSE_PAY;
            }
            else if (numFlush == 1)
            {
                return FLUSH_PAY;
            }
            else if (numStraight == 1)
            {
                return STRAIGHT_PAY;
            }
            else if (numThreeOfKind == 1)
            {
                return THREE_OF_KIND_PAY;
            }
            else if (numTwoPair == 1)
            {
                return TWO_PAIR_PAY;
            }
            else if (numPair == 1)
            {
                return PAIR_PAY;
            }
            else
            {
                return 0;
            }
        }
        static int CheckPair(Card[] hand, int numThreeOfKind, int numFourOfKind, int numFullHouse, int numTwoPair)
        {
            if (numFourOfKind == 1 || numFullHouse == 1 || numThreeOfKind == 1 || numTwoPair == 1)
            {
                return 0;
            }
            else
            {
                if (hand[0].getFaceValue() == hand[1].getFaceValue() && hand[0].getFaceValue() > Card.FaceValue.Ten)
                {
                    return 1;
                }
                else if (hand[1].getFaceValue() == hand[2].getFaceValue() && hand[1].getFaceValue() > Card.FaceValue.Ten)
                {
                    return 1;
                }
                else if (hand[2].getFaceValue() == hand[3].getFaceValue() && hand[2].getFaceValue() > Card.FaceValue.Ten)
                {
                    return 1;
                }
                else if (hand[3].getFaceValue() == hand[4].getFaceValue() && hand[3].getFaceValue() > Card.FaceValue.Ten)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
        static int CheckTwoPair(Card[] hand, int numFourOfKind, int numFullHouse, int numThreeOfKind)
        {
            if (numFourOfKind == 1 || numFullHouse == 1 || numThreeOfKind == 1)
            {
                return 0;
            }
            else
            {
                if (hand[0].getFaceValue() == hand[1].getFaceValue() && hand[2].getFaceValue() == hand[3].getFaceValue())
                {
                    return 1;
                }
                else if (hand[0].getFaceValue() == hand[1].getFaceValue() && hand[3].getFaceValue() == hand[4].getFaceValue())
                {
                    return 1;
                }
                else if (hand[1].getFaceValue() == hand[2].getFaceValue() && hand[3].getFaceValue() == hand[4].getFaceValue())
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
        static int CheckThreeOfKind(Card[] hand, int numFourOfKind, int numFullHouse)
        {
            if (numFourOfKind == 1 || numFullHouse == 1)
            {
                return 0;
            }
            else
            {
                if (hand[0].getFaceValue() == hand[1].getFaceValue() && hand[1].getFaceValue() == hand[2].getFaceValue())
                {
                    return 1;
                }
                else if (hand[1].getFaceValue() == hand[2].getFaceValue() && hand[2].getFaceValue() == hand[3].getFaceValue())
                {
                    return 1;
                }
                else if (hand[2].getFaceValue() == hand[3].getFaceValue() && hand[3].getFaceValue() == hand[4].getFaceValue())
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
        static int CheckFourOfKind(Card[] hand)
        {
            if (hand[0].getFaceValue() == hand[1].getFaceValue() && hand[1].getFaceValue() == hand[2].getFaceValue() && hand[2].getFaceValue() == hand[3].getFaceValue())
            {
                return 1;
            }
            else if (hand[1].getFaceValue() == hand[2].getFaceValue() && hand[2].getFaceValue() == hand[3].getFaceValue() && hand[3].getFaceValue() == hand[4].getFaceValue())
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        static int CheckStraight(Card[] hand)
        {
            int matchCounter = 0;
            for (int index = 0; index < HAND_SIZE - 1; index++)
            {
                if (hand[index].getFaceValue() + 1 == hand[index + 1].getFaceValue())
                {
                    matchCounter++;
                }
                else
                {
                    return 0;
                }
            }
            if (matchCounter == 4)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        static int CheckFlush(Card[] hand)
        {
            if (hand[0].getSuit() == hand[1].getSuit() && hand[1].getSuit() == hand[2].getSuit() && hand[2].getSuit() == hand[3].getSuit() && hand[3].getSuit() == hand[4].getSuit())
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        static int CheckFullHouse(Card[] hand)
        {
            if (hand[0].getFaceValue() == hand[1].getFaceValue() && hand[1].getFaceValue() == hand[2].getFaceValue() && hand[3].getFaceValue() == hand[4].getFaceValue())
            {
                return 1;
            }
            else if (hand[0].getFaceValue() == hand[1].getFaceValue() && hand[2].getFaceValue() == hand[3].getFaceValue() && hand[3].getFaceValue() == hand[4].getFaceValue())
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        // =====================================================================================         
        // Give feedback to the user whether it was a winning or losing hand.
        // Display the amount won or loss and the amount left in the bankroll.
        // ===================================================================================== 
        static void WinOrLose(int amountToMultiply, ref int bankroll, int amountWon, int bet, ref bool gameOver)
        {
            if (amountToMultiply == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You have no winning hand.");
                Console.ResetColor();
                DisplayAmountWon(amountWon);
                DisplayBankroll(bankroll);
                // End the game if there is no money left
                if (bankroll == 0)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No more money left in the bank, you lose!");
                    Console.ResetColor();
                    Console.WriteLine();
                    gameOver = true;
                }
                else
                {
                    gameOver = IsGameOver();
                    if (gameOver)
                    {
                        DisplayGameOverMessage(bankroll);
                    }
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Winning hand: " + WinMessage(amountToMultiply));
                Console.ResetColor();
                Console.WriteLine();
                amountWon = DisplayAndAddMoney(amountToMultiply, bet, ref bankroll);
                DisplayAmountWon(amountWon);
                DisplayBankroll(bankroll);
                gameOver = IsGameOver();
                if (gameOver)
                {
                    DisplayGameOverMessage(bankroll);
                }
            }
        }
        // When user is done playing, display a thank you message and the amount left in the bank.
        static void DisplayGameOverMessage(int bankroll)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Thank you for playing, you leave with " + bankroll.ToString("C") + " in the bank");
            Console.ResetColor();
        }       
        // Ask the user if he wants to keep playinghow 
        static bool IsGameOver()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Do you wish to play again ? Press any key to continue or \"Q\" to quit.");
            Console.ResetColor();
            string input = Console.ReadLine();
            if (input.ToLower() == "q")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        // Display win message based on the winning hand
        static string WinMessage(int amountToMultiply)
        {
            switch (amountToMultiply)
            {
                case ROYAL_FLUSH_PAY:
                    return "Royal Flush";
                case STRAIGHT_FLUSH_PAY:
                    return "Straight Flush";
                case FOUR_OF_KIND_PAY:
                    return "Four of a Kind";
                case FULL_HOUSE_PAY:
                    return "Full House";
                case FLUSH_PAY:
                    return "Flush";
                case STRAIGHT_PAY:
                    return "Straight";
                case THREE_OF_KIND_PAY:
                    return "Three of a Kind";
                case TWO_PAIR_PAY:
                    return "Two Pair";
                case PAIR_PAY:
                    return "Pair";
            }
            return "";
        }
        // Display the amount won
        static void DisplayAmountWon(int amountWon)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Money won: " + amountWon.ToString("C"));
            Console.ResetColor();
        }

        // =====================================================================================         
        // All methods for TEST
        // ===================================================================================== 
        static int TestingWinConditions(int choice)
        {
            Console.WriteLine("\n1. Test Royal Flush \n2. Test Straight Flush \n3. Test Four of a Kind \n4. Test Full House \n5. Test Flush \n6. Test Straight \n7. Test Three of a Kind \n8. Test 2 Pair \n9. Test Pair \n0. Main Menu \n");
            Console.Write("Please enter your desired option: ");
            string input = Console.ReadLine();
            while (!int.TryParse(input, out choice) || int.Parse(input) > 9 || int.Parse(input) < 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Error, please enter a valid menu choice: ");
                Console.ResetColor();
                input = Console.ReadLine();
            }
            switch (choice)
            {
                case 0:
                    break;
                case 1:
                    TestRoyalFlush();
                    break;
                case 2:
                    TestStraightFlush();
                    break;
                case 3:
                    TestFourOfaKind();
                    break;
                case 4:
                    TestFullHouse();
                    break;
                case 5:
                    TestFlush();
                    break;
                case 6:
                    TestStraight();
                    break;
                case 7:
                    TestThreeOfaKind();
                    break;
                case 8:
                    TestTwoPair();
                    break;
                case 9:
                    TestPair();
                    break;
            }
            return choice;
        }
        // The only difference between this method and WinOrLose method is that it doesnt have the IsGameOver method. (The method that ask if the user wants to play again)
        static void WinOrLoseTESTING(int amountToMultiply, int bankroll, int amountWon, int bet)
        {
            if (amountToMultiply == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You have no winning hand.");
                Console.ResetColor();
                DisplayAmountWon(amountWon);
                DisplayBankroll(bankroll);
                if (bankroll == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No more money left in the bank, you lose!");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Winning hand: " + WinMessage(amountToMultiply));
                Console.ResetColor();
                Console.WriteLine();
                amountWon = DisplayAndAddMoney(amountToMultiply, bet, ref bankroll);
                DisplayAmountWon(amountWon);
                DisplayBankroll(bankroll);
            }
        }
        static void TestPair()
        {
            int bankroll = INIT_AMT_BANKROLL, bet = 0, amountToMultiply = 0, amountWon = 0;
            AskBetAndDisplayBankroll(ref bankroll, ref bet);
            Card[] hand = new Card[HAND_SIZE];
            hand[0] = new Card(Card.Suit.Spades, Card.FaceValue.Three);
            hand[1] = new Card(Card.Suit.Spades, Card.FaceValue.Four);
            hand[2] = new Card(Card.Suit.Spades, Card.FaceValue.Ten);
            hand[3] = new Card(Card.Suit.Spades, Card.FaceValue.Jack);
            hand[4] = new Card(Card.Suit.Hearts, Card.FaceValue.Jack);
            DisplayHand(hand);
            amountToMultiply = CheckWinCondition(hand);
            WinOrLoseTESTING(amountToMultiply, bankroll, amountWon, bet);
        }
        static void TestTwoPair()
        {
            int bankroll = INIT_AMT_BANKROLL, bet = 0, amountToMultiply = 0, amountWon = 0;
            AskBetAndDisplayBankroll(ref bankroll, ref bet);
            Card[] hand = new Card[HAND_SIZE];
            hand[0] = new Card(Card.Suit.Hearts, Card.FaceValue.Six);
            hand[1] = new Card(Card.Suit.Spades, Card.FaceValue.Six);
            hand[2] = new Card(Card.Suit.Diamonds, Card.FaceValue.Eight);
            hand[3] = new Card(Card.Suit.Spades, Card.FaceValue.Eight);
            hand[4] = new Card(Card.Suit.Spades, Card.FaceValue.Ten);
            DisplayHand(hand);
            amountToMultiply = CheckWinCondition(hand);
            WinOrLoseTESTING(amountToMultiply, bankroll, amountWon, bet);
        }
        static void TestThreeOfaKind()
        {
            int bankroll = INIT_AMT_BANKROLL, bet = 0, amountToMultiply = 0, amountWon = 0;
            AskBetAndDisplayBankroll(ref bankroll, ref bet);
            Card[] hand = new Card[HAND_SIZE];
            hand[0] = new Card(Card.Suit.Diamonds, Card.FaceValue.Four);
            hand[1] = new Card(Card.Suit.Clubs, Card.FaceValue.Four);
            hand[2] = new Card(Card.Suit.Hearts, Card.FaceValue.Four);
            hand[3] = new Card(Card.Suit.Spades, Card.FaceValue.Seven);
            hand[4] = new Card(Card.Suit.Spades, Card.FaceValue.Nine);
            DisplayHand(hand);
            amountToMultiply = CheckWinCondition(hand);
            WinOrLoseTESTING(amountToMultiply, bankroll, amountWon, bet);
        }
        static void TestStraight()
        {
            int bankroll = INIT_AMT_BANKROLL, bet = 0, amountToMultiply = 0, amountWon = 0;
            AskBetAndDisplayBankroll(ref bankroll, ref bet);
            Card[] hand = new Card[HAND_SIZE];
            hand[0] = new Card(Card.Suit.Spades, Card.FaceValue.Six);
            hand[1] = new Card(Card.Suit.Spades, Card.FaceValue.Seven);
            hand[2] = new Card(Card.Suit.Diamonds, Card.FaceValue.Eight);
            hand[3] = new Card(Card.Suit.Spades, Card.FaceValue.Nine);
            hand[4] = new Card(Card.Suit.Hearts, Card.FaceValue.Ten);
            DisplayHand(hand);
            amountToMultiply = CheckWinCondition(hand);
            WinOrLoseTESTING(amountToMultiply, bankroll, amountWon, bet);
        }
        static void TestFlush()
        {
            int bankroll = INIT_AMT_BANKROLL, bet = 0, amountToMultiply = 0, amountWon = 0;
            AskBetAndDisplayBankroll(ref bankroll, ref bet);
            Card[] hand = new Card[HAND_SIZE];
            hand[0] = new Card(Card.Suit.Spades, Card.FaceValue.Two);
            hand[1] = new Card(Card.Suit.Spades, Card.FaceValue.Four);
            hand[2] = new Card(Card.Suit.Spades, Card.FaceValue.Five);
            hand[3] = new Card(Card.Suit.Spades, Card.FaceValue.Seven);
            hand[4] = new Card(Card.Suit.Spades, Card.FaceValue.Ten);
            DisplayHand(hand);
            amountToMultiply = CheckWinCondition(hand);
            WinOrLoseTESTING(amountToMultiply, bankroll, amountWon, bet);
        }
        static void TestFullHouse()
        {
            int bankroll = INIT_AMT_BANKROLL, bet = 0, amountToMultiply = 0, amountWon = 0;
            AskBetAndDisplayBankroll(ref bankroll, ref bet);
            Card[] hand = new Card[HAND_SIZE];
            hand[0] = new Card(Card.Suit.Spades, Card.FaceValue.Ten);
            hand[1] = new Card(Card.Suit.Diamonds, Card.FaceValue.Ten);
            hand[2] = new Card(Card.Suit.Hearts, Card.FaceValue.Ten);
            hand[3] = new Card(Card.Suit.Hearts, Card.FaceValue.Queen);
            hand[4] = new Card(Card.Suit.Spades, Card.FaceValue.Queen);
            DisplayHand(hand);
            amountToMultiply = CheckWinCondition(hand);
            WinOrLoseTESTING(amountToMultiply, bankroll, amountWon, bet);
        }
        static void TestFourOfaKind()
        {
            int bankroll = INIT_AMT_BANKROLL, bet = 0, amountToMultiply = 0, amountWon = 0;
            AskBetAndDisplayBankroll(ref bankroll, ref bet);
            Card[] hand = new Card[HAND_SIZE];
            hand[0] = new Card(Card.Suit.Spades, Card.FaceValue.Ten);
            hand[1] = new Card(Card.Suit.Hearts, Card.FaceValue.Ten);
            hand[2] = new Card(Card.Suit.Clubs, Card.FaceValue.Ten);
            hand[3] = new Card(Card.Suit.Diamonds, Card.FaceValue.Ten);
            hand[4] = new Card(Card.Suit.Spades, Card.FaceValue.Ace);
            DisplayHand(hand);
            amountToMultiply = CheckWinCondition(hand);
            WinOrLoseTESTING(amountToMultiply, bankroll, amountWon, bet);
        }
        static void TestStraightFlush()
        {
            int bankroll = INIT_AMT_BANKROLL, bet = 0, amountToMultiply = 0, amountWon = 0;
            AskBetAndDisplayBankroll(ref bankroll, ref bet);
            Card[] hand = new Card[HAND_SIZE];
            hand[0] = new Card(Card.Suit.Spades, Card.FaceValue.Two);
            hand[1] = new Card(Card.Suit.Spades, Card.FaceValue.Three);
            hand[2] = new Card(Card.Suit.Spades, Card.FaceValue.Four);
            hand[3] = new Card(Card.Suit.Spades, Card.FaceValue.Five);
            hand[4] = new Card(Card.Suit.Spades, Card.FaceValue.Six);
            DisplayHand(hand);
            amountToMultiply = CheckWinCondition(hand);
            WinOrLoseTESTING(amountToMultiply, bankroll, amountWon, bet);
        }
        static void TestRoyalFlush()
        {
            int bankroll = INIT_AMT_BANKROLL, bet = 0, amountToMultiply = 0, amountWon = 0;
            AskBetAndDisplayBankroll(ref bankroll, ref bet);
            Card[] hand = new Card[HAND_SIZE];
            hand[0] = new Card(Card.Suit.Spades, Card.FaceValue.Ten);
            hand[1] = new Card(Card.Suit.Spades, Card.FaceValue.Jack);
            hand[2] = new Card(Card.Suit.Spades, Card.FaceValue.Queen);
            hand[3] = new Card(Card.Suit.Spades, Card.FaceValue.King);
            hand[4] = new Card(Card.Suit.Spades, Card.FaceValue.Ace);
            DisplayHand(hand);
            amountToMultiply = CheckWinCondition(hand);
            WinOrLoseTESTING(amountToMultiply, bankroll, amountWon, bet);

        }
    }
}
