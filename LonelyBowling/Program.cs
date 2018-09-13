
namespace LonelyBowling
{
    using System;
    using System.Reflection;
    using System.Media;
    using System.Threading;

    class Program
    {
        public static SoundPlayer sp = new SoundPlayer();
        //dynamically finds filepath based on where the executable is located.
        //folder path = folder name of applications location
        //uses "using System.Reflection;"
        private static string folderpath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        static int ObtainInput(string message)
        {
            //do not leave method unless tryparse is valid
            int score;
            bool validInput = false;

            do
            {
                Console.Write(message);
                string input = Console.ReadLine();
                validInput = int.TryParse(input, out score);
                if (!validInput)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid entry!");
                    Console.ResetColor();
                }

            }   while (!validInput);
            return score;
        }

        public static void initializeSounds()
        {
            //strike
            sp.SoundLocation= folderpath + @"\DataSources\bowling sounds\strike+2.wav";
            sp.Load();
            //spare
            sp.SoundLocation = folderpath + @"\DataSources\bowling sounds\yell4yeeha.wav";
            sp.Load();
            //turkey
            sp.SoundLocation = folderpath + @"\DataSources\bowling sounds\Turkey Gobble-SoundBible.com-123256561.wav";
            sp.Load();
            //applause
            //great game
            sp.SoundLocation = folderpath + @"\DataSources\bowling sounds\applause2.wav";
            sp.Load();
            //bad game
            sp.SoundLocation = folderpath + @"\DataSources\bowling sounds\cwdboo.wav";
            sp.Load();
            //gutterball
            sp.SoundLocation = folderpath + @"\DataSources\bowling sounds\bowling1.wav";
            sp.Load();
            //Horror
            sp.SoundLocation = folderpath + @"\DataSources\bowling sounds\Scream+1.wav";
            sp.Load();
        }
            

        static void Main(string[] args)
        {
            int score = 0;

            int[][] inputs = new int[10][];

            for (int i =0; i <=8; i++)
                inputs[i] = new int[2];

            inputs[9] = new int[3];

            for(int frame = 0; frame <=8; frame++)
            {
                bool valid;
                
                    int ball1;
                    int ball2=0;
                
                do
                {
                    ball1 = ObtainInput("Ball 1: ");

                    if (!(ball1 >= 0 && ball1 <= 10))
                        valid = false;
                    else
                        valid = true;

                } while (!valid);

                if (ball1 == 0)
                {
                    sp.SoundLocation = folderpath + @"\DataSources\bowling sounds\bowling1.wav";
                    sp.Play();
                    Thread.Sleep(1500);
                }


                if (!(ball1 == 10))
                {
                    do
                    {
                        ball2 = ObtainInput("Ball 2: ");

                        if (ball2 > (10 - ball1) || (ball2 < 0))
                            valid = false;
                        else
                            valid = true;

                    } while (!valid);
                }

                if (ball2 == 0)
                {
                    sp.SoundLocation = folderpath + @"\DataSources\bowling sounds\bowling1.wav";
                    sp.Play();
                    Thread.Sleep(1500);
                }


                Console.Clear();

                if (ball1 == 10)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("STRIKE!!!!");
                    Console.ResetColor();
                    sp.SoundLocation = folderpath + @"\DataSources\bowling sounds\strike+2.wav";
                    sp.Play();


                }
                else if (ball1 + ball2 == 10)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Spare!");
                    Console.ResetColor();
                    sp.SoundLocation = folderpath + @"\DataSources\bowling sounds\yell4yeeha.wav";
                    sp.Play();
                }
                        

                inputs[frame][0] = ball1;
                inputs[frame][1] = ball2;

                score = TotalScore(inputs);
                Console.WriteLine("    "+score+"\n");

            }
            
            inputs[9] = finalFrame();

            score = TotalScore(inputs);
            if(score==300)
            {
                //perfect game
                Console.ForegroundColor = ConsoleColor.Green;
                //player play sound?
                Console.WriteLine("PERFECT GAME!!!!");
                sp.SoundLocation = folderpath + @"\DataSources\bowling sounds\Turkey Gobble-SoundBible.com-123256561.wav";
                sp.Play();

            }
            else if (score >200)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                //great game
                Console.WriteLine("Great Game!");
                sp.SoundLocation = folderpath + @"\DataSources\bowling sounds\applause2.wav";
                sp.Play();

            }
            else if (score >100)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                //not bad
                Console.WriteLine("Not Bad :)");
                sp.SoundLocation = folderpath + @"\DataSources\bowling sounds\yell4yeeha.wav";
                sp.Play();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("I hope your not driving");
                sp.SoundLocation = folderpath + @"\DataSources\bowling sounds\Scream+1.wav";
                sp.Play();

            }
            Console.WriteLine("    " + score);
            Console.ResetColor();

            Console.ReadKey();
        }

        public static int TotalScore(int[][] inputs)
        {
            int score = 0;

            //index's through frames
            for (int frame = 0; frame <= 9; frame++)
            {
                int frameScore = 0;
                int length = inputs[frame].GetLength(0);
                //checks for last 2 frames
                if (frame < 8)
                {
                    //checks first roll is strike, sets framescore to 10
                    if (inputs[frame][0] == 10)
                    {
                        frameScore = 10;
                        //checks for second strike
                        if (inputs[frame + 1][0] == 10)
                            //second strike is true, adds 10 + next roll
                            frameScore += (10 + inputs[frame + 2][0]);
                        else
                            //second strike is false, adds rolls of frame+1
                            frameScore += (inputs[frame+1][0] + inputs[frame + 1][1]);
                    }
                    //checks frame is spare, adds first roll of next frame, + 10
                    else if (inputs[frame][0] + inputs[frame][1] == 10)
                    {
                        frameScore = 10+inputs[frame+1][0];
                    }
                    //neither strike nor spare, adds both rolls to framescore
                    else
                    {
                        //add each roll of frame to framescore
                        frameScore += inputs[frame][0] + inputs[frame][1];
                    }
                }
                //checking for 9th game frame
                else if(frame==8)
                {
                    if (inputs[frame][0] == 10)
                    {
                        frameScore += 10;
                        frameScore += inputs[frame + 1][0] + inputs[frame + 1][1];
                    }
                    else if (inputs[frame][0] + inputs[frame][1] == 10)
                    {
                        frameScore += (10 + inputs[frame + 1][0]);
                    }
                    else
                        frameScore += (inputs[frame][0] + inputs[frame][1]);
                }
                //not in the first 9 frames, then this is the 10th frame.
                //just need to add the 3 rolls
                else
                {
                    frameScore += inputs[frame][0]+inputs[frame][1]+inputs[frame][2];
                }
                //add each frames score to total score
                score += frameScore;
            }

            return score;
        }

        public static int[] finalFrame()
        {
            int[] inputs = new int[3];
            int ball1 = 0;
            int ball2 = 0;
            int ball3 = 0;

            bool valid;

            do
            {
                ball1 = ObtainInput("Ball 1: ");

                if ((ball1 >= 0 && ball1 <= 10))
                    valid = false;
                else
                    valid = true;

            } while (valid) ;

            if (ball1 == 10)
            {
                do
                {
                    ball2 = ObtainInput("Ball 2: ");

                    if (ball2<=10&&ball2>=0)
                        valid = false;
                    else
                        valid = true;

                } while (valid);
            }
            else
            {
                do
                {
                    ball2 = ObtainInput("Ball 2: ");

                    if (ball2<=(10-ball1) && ball2>=0)
                        valid = false;
                    else
                        valid = true;

                } while (valid);
            }

            if ((ball1 + ball2) >= 10)
            {
                if (ball2 == 10)
                {

                    do
                    {
                        ball3 = ObtainInput("Ball 3: ");

                        if (ball3 <= 10 || ball2 >= 0)
                            valid = false;
                        else
                            valid = true;

                    } while (valid);
                }
                else
                {
                    do
                    {
                        ball3 = ObtainInput("Ball 3: ");

                        if (ball3 <= (10 - ball2) && ball3 >= 0)
                            valid = false;
                        else
                            valid = true;

                    } while (valid);
                }
            }

            inputs[0] = ball1;
            inputs[1] = ball2;
            inputs[2] = ball3;

            Console.Clear();

            return inputs;
        }
    }
}
