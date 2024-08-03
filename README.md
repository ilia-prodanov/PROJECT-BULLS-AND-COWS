# *PROJECT* 🐮 *BULLS AND COWS* 🐮
Welcome, everybody!🤩🤩🤩

This game is about guessing a secret 4-digit code. The game is very similar to "Mastermind" from 1970 but has more variety: instead of 4 positions and 6 colored pegs (digits in the case) we have 4 positions and 10 possible digits (including 0). Two players think of personal secret code and take turns to guess the secret code of the opponent. If the guess-number for has a common column with the secret number, there is a bull. If the guess-number has a common digit but on a different position, there is a cow. You can see more on the link here: https://en.wikipedia.org/wiki/Bulls_and_cows

I wanted to make a personal project with program that has intelligent algorhytm that you could play with. First I relied on descision-tree logic until I realised that It's not good enough... Thankfully I found a very smart guy called  Donald Knuth who inspired me (we'll get to that later).
The result is...
> [!NOTE]
> ...аn application using some **heavy ass** logic in order to make a competitive companion to play the game with...

How competitive? VERY competitive. After many hours of coding and debugging I finally have a complete algorhytm that is as capable as a human opponent. Or even more 😈... 
According to Knuth the algorhytm will guess the **Mastermind** code in no more than 5 turns. This is the worst possible scenario and so is the method called - Minimax method.
> [!NOTE]
>  The very same method that I used to create a program that will guess the secret code in no more than 9 turns (because the method is created for the **Mastermind** game and not for the **BULLS AND COWS**. Why 9 though? Is there some more **heavy ass** mathematician-get-me-to-Harvard-logic?
> No😆. It's pure homo-stupidiens making a lot of manual testing. According to the mentioned experience, 9 is probably the worst case.  

Okay, so... is that it? 

No! And I'll tell you why...

> [!CAUTION]
> During testing I found that there are cases when the algorhytm finds out 3 of the 4 bulls lightning fast but then it gets almost stuck not knowing which is the last bull, the last valid number. The secret number was 1912 and it guessed with 1312. After I told the program that there are 3
> bulls, there were only several options left: 1412, 1512, 1612... you get it. It couldn't optimise the process of finding out the last secret digit so it started asking me every mentioned number until the guess was correct🤦‍♂️. You can imagine how slow the overall result was starting from 0 to 3 bulls in 3 turns and from 3 to 4 bulls in 6 more...

> [!TIP]
> So I decided to make an additinal algorhytm for optimising the first algorhytm. And it worked. It saved on average 2 turns and now instead of 9 turns to guess 1912 it needs 7 turns. Which is great by the way. The code become double it's previous size with me beginning to wonder if I was ever going to make it (after debugging basically everything everywhere, even my life after that😉)

That's it, guys. Thanks for reading, everybody!

![image](https://github.com/user-attachments/assets/4c2355d5-64f1-4b54-a8cf-eabf7163ed5c)


