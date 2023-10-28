using RobotController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllPawnsMustDie.core
{
    internal class CheRo
    {

        // Static instance variable
        private static CheRo instance;

        // Private constructor to prevent instantiation outside the class
        private CheRo() 
        {
            //this.control = new RobotControl();
            //start();
        }

        // Public method to access the singleton instance
        public static CheRo Instance
        {
            get
            {
                // Create the instance if it doesn't exist
                if (instance == null)
                {
                    instance = new CheRo();
                }
                return instance;
            }
        }


        private RobotControl control;
        private const int fieldLengthMM = 35;
        private const int dropLengthMM = 47;
        private (int x, int y) disposal = (12, 1);

        public void start()
        {
            this.control.StartConnection();
            initBase();
        }

        public void stop()
        {
            this.control.CloseConnection();
        }

        public void moveFromTo(MoveInformation moveInformation)
        {
            Console.WriteLine("=====================================================");
            Console.WriteLine("Received Move! Yeah! :D");
            if (moveInformation.IsCapture) // first get rid of the captured piece
            {
                Console.WriteLine("Capture Move detected");
                dispose(moveInformation);
            } else
            {
                Console.WriteLine("Capture Move NOT detected");
            }
            //MoveRobotLin(moveInformation.Start);
            Console.WriteLine("Moved to: " + moveInformation.Start);
            //grab();
            Console.WriteLine("grabbed se piece at: " + moveInformation.Start);
            //MoveRobotLin(moveInformation.End);
            Console.WriteLine("Moved to: " + moveInformation.End);
            //putDown();
            Console.WriteLine("released se piece at: " + moveInformation.End);
            Console.WriteLine("=====================================================");
        }

        private void dispose(MoveInformation moveInformation)
        {
            PieceFile file = moveInformation.CapturedPiece.File;
            int rank = moveInformation.CapturedPiece.Rank;
            BoardSquare square = new BoardSquare(file, rank);
            Console.WriteLine("Captured Piece detected at location: " + square);
            //MoveRobotLin(square); // move to taken piece
            Console.WriteLine("moved to: " + square);
            //grab(); // grab it
            Console.WriteLine("grabbed dead piece. RIP");
            //MoveRobotLin(disposal); // move to disposal area
            Console.WriteLine("moved to OBLIVIONN!!!!");
            //putDown(); // release to oblivion!!
            Console.WriteLine("released dead piece into OBLIVIONN!!!!!");

        }


        private void grab()
        {
            control.MoveTCPToPosition(new RobotCartPosition(dropLengthMM, 0, 0, 0, 0, 0), RobotCartMoveType.LIN, true);
            control.CloseGripper();
            control.MoveTCPToPosition(new RobotCartPosition(-dropLengthMM, 0, 0, 0, 0, 0), RobotCartMoveType.LIN, true);
        }

        private void putDown()
        {
            control.MoveTCPToPosition(new RobotCartPosition(dropLengthMM, 0, 0, 0, 0, 0), RobotCartMoveType.LIN, true);
            control.OpenGripper();
            control.MoveTCPToPosition(new RobotCartPosition(-dropLengthMM, 0, 0, 0, 0, 0), RobotCartMoveType.LIN, true);
        }

        private void MoveRobotLin(BoardSquare square)
        {
            RobotCartPosition rcp = new RobotCartPosition(0, -(square.File.ToInt() - 1) * fieldLengthMM, (square.Rank - 1) * fieldLengthMM, 0, 0, 0);
            control.MoveTCPToPosition(rcp, RobotCartMoveType.LIN, false);

        }

        private void MoveRobotLin((int x, int y) p)
        {
            RobotCartPosition rcp = new RobotCartPosition(0, -(p.x - 1) * fieldLengthMM, (p.y - 1) * fieldLengthMM, 0, 0, 0);
            control.MoveTCPToPosition(rcp, RobotCartMoveType.LIN, false);
        }

        private void initBase()
        {
            int status = 6;
            control.MoveAxesToPosition(new RobotAxisPosition(0, -90, 90, 0, -10, 0));
            control.SetBase(3);
            control.SetTool(3);
            control.MoveTCPToPosition(new RobotCartPosition(470, 0, 580, -122, 90, -122, status), RobotCartMoveType.PTP, false);
            control.SetBase(4);
            control.MoveTCPToPosition(new RobotCartPosition(0, 0, 0, 0, 0, 0, status), RobotCartMoveType.PTP, false);
        }

    }
}
