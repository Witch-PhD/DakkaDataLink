using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PitBoss
{
    internal class SpotterKeystrokeHandler
    {
        DataManager dataManager = DataManager.Instance;
        private bool m_IsActive = false;
        bool rightCtrlHeldDown = false;
        bool rightShiftHeldDown = false;

        double smallDistIncrement = 1.0;
        double largeDistIncrement = 10.0;

        double smallAzIncrement = 1.0;
        double largeAzIncrement = 15.0;

        double azIncrement;
        double distIncrement;

        private static SpotterKeystrokeHandler? m_instance;
        public static SpotterKeystrokeHandler Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new SpotterKeystrokeHandler();
                }
                return m_instance;
            }
            private set { }
        }

        SpotterKeystrokeHandler()
        {
            azIncrement = smallAzIncrement;
            distIncrement = smallDistIncrement;
        }

        public void Activate()
        {
            m_IsActive = true;
        }

        public void Deactivate()
        {
            m_IsActive = false;
        }

        internal void handleKeyDown(RawKeyEventArgs args)
        {
            if (!m_IsActive)
            {
                return;
            }
           // Console.WriteLine("Key Down: " + args.Key.ToString());
            //////// Modifiers ////////
            if (args.Key == Key.RightShift)
            {
                rightShiftHeldDown = true;
            }

            else if (args.Key == Key.RightCtrl)
            {
                rightCtrlHeldDown = true;
            }

            if (rightShiftHeldDown)
            {
                azIncrement = largeAzIncrement;
                distIncrement = largeDistIncrement;
            }
            else if (rightCtrlHeldDown)
            {
                azIncrement = smallAzIncrement;
                distIncrement = smallDistIncrement;
            }
            else
            {
                return;
            }

            // TODO: Make this a switch statement.

            //////// Distance ////////
            if (args.Key == Key.Up)
            {
                dataManager.LatestDist += distIncrement;
            }
            else if (args.Key == Key.Down)
            {
                dataManager.LatestDist -= distIncrement;
            }

            //////// Azimuth ////////
            else if (args.Key == Key.Right)
            {
                dataManager.LatestAz += azIncrement;
            }
            else if (args.Key == Key.Left)
            {
                dataManager.LatestAz -= azIncrement;
            }

            //////// Send New Coords ////////
            else if (args.Key == Key.NumPad0)
            {
                if (rightCtrlHeldDown)
                {
                    dataManager.SendCoords();
                }
            }

        }

        internal void handleKeyUp(RawKeyEventArgs args)
        {
            if (!m_IsActive)
            {
                return;
            }
            //Console.WriteLine("Key Up: " + args.Key.ToString());
            if (args.Key == Key.RightShift)
            {
                rightShiftHeldDown = false;
            }
            else if (args.Key == Key.RightCtrl)
            {
                rightCtrlHeldDown = false;
            }
        }
    }
}
