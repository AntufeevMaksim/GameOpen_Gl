﻿
class Program
{
  static void Main()
  {
    using (Game game = new Game(1920, 1080, "LearnOpenTK"))
    {
      game.Run();
    }    
  }
}