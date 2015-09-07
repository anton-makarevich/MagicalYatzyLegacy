using NGraphics;
using Sanet.Kniffel.Models;
using Sanet.Kniffel.XF.Models;
using Sanet.Kniffel.XF.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Sanet.Kniffel.DicePanel

{

    /// <summary>
    /// Dice Panel Control - where we are rolling dices
    /// </summary>
    public class DicePanel : NControl.Abstractions.NControlView
    {
        #region Fields
        //popup to show dice value selection panel
        //Popup _popup = new Popup();
        //DiceValueSelectionPanel _selectionPanel = new DiceValueSelectionPanel();

        //To show messages
        string caption;// = new TextBlock();

        IPlatform _platform;
        private DiceStyle FStyle = DiceStyle.dpsClassic;
        public List<Die> aDice = new List<Die>();
        public Random FRand = new Random();
        private int FDieAngle = 0;
        private bool _WithSound = false;
        private int FMaxRollLoop = 75;
        private int FNumDice = 0;
        private bool FDebugDrawMode = false;
        private bool FClickToFreeze;
        private int _RollDelay;
        Die _lastClickedDie;
        private bool _ManualSetMode = false;

        //cashed images
        public Dictionary<string, IImage> DieFrameImages = new Dictionary<string, IImage>();

        const string strImageRoot = "Images/";

        public string strStyle;
        private Storyboard sbLoop;
        private Color dpBackcolor = Colors.LightGray;


        #endregion

        #region Events
        public delegate void DieFrozenEventHandler(bool @fixed, int Value);
        public delegate void DieChangedEventHandler(bool isfixed, int oldvalue,int newvalue);
        public delegate void DieBouncedEventHandler();
        public delegate void EndRollEventHandler();
        public delegate void BeginRollEventHandler();
        public delegate void StartLoadingEventHandler();
        public delegate void StopLoadingEventHandler();

        public event DieBouncedEventHandler DieBounced;
        public event DieFrozenEventHandler DieFrozen;
        public event DieChangedEventHandler DieChangedManual;
        public event EndRollEventHandler EndRoll;
        public event BeginRollEventHandler BeginRoll;

        public event Action<bool> PanelIsBusy;
        #endregion

        #region Properties
        public static double DeviceScale=1;
        public bool TreeDScale { get; set; }
        public double TreeDScaleCoef { get; set; }
        public bool PlaySound { get; set; }
        public DiceStyle PanelStyle
        {
            get { return FStyle; }
            
        }
        public int DieAngle
        {
            get { return FDieAngle; }
            set
            {
                if (value < 0)
                    value = 0;
                if (value > 5)
                    value = 5;
                FDieAngle = value;
                foreach (Die d in aDice)
                    d.DrawDie();
                Invalidate();
            }
        }
        public int MaxRollLoop
        {
            get { return FMaxRollLoop; }
            set
            {
                if (value < 20)
                    value = 20;
                if (value > 150)
                    value = 150;
                FMaxRollLoop = value;
            }
        }
        /// <summary>
        /// Number of Dice in the Panel
        /// </summary>
        public int NumDice
        {
            get { return FNumDice; }
            set
            {
                FNumDice = value;

                GenerateDice();

            }
        }
        /// <summary>
        /// Draws a Box Around the Die for Collision Debugging
        /// </summary>
        public bool DebugDrawMode
        {
            get { return FDebugDrawMode; }
            set { FDebugDrawMode = value; }
        }
        /// <summary>
        /// Allows user to click dice to lock their movement
        /// </summary>
        public bool ClickToFreeze
        {
            get { return FClickToFreeze; }
            set { FClickToFreeze = value; }
        }

        /// <summary>
        /// Summed Result of All the Dice
        /// </summary>
        public DieResult Result
        {
            get
            {
                var dr = new List<int>();

                foreach (Die d in aDice)
                {
                    dr.Add(d.Result);

                }
                return new DieResult { DiceResults = dr };
            }
        }

        //new, changed visibility
        public bool AllDiceStopped
        {
            get
            {
                foreach (Die d in aDice)
                {
                    if (d.IsRolling)
                        return false;
                }

                return true;
            }
        }

        bool isRolling
        {
            get
            {
                foreach (Die d in aDice)
                    if (d.IsRolling) return true;
                return false;
            }
        }

        public bool WithSound
        {
            get { return _WithSound; }
            set
            {
                if (_WithSound != value)
                {
                    _WithSound = value;

                }
            }
        }

        public int RollDelay
        {
            get { return _RollDelay; }
            set { _RollDelay = value; }
        }

        public bool ManualSetMode
        {
            get { return _ManualSetMode; }
            set
            {
                if (_ManualSetMode != value)
                {
                    _ManualSetMode = value;
                    /*if (value)
                    {
                        caption.Text = "SelectDiceToChangeMessage".Localize();
                        caption.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        caption.Text = string.Empty;
                        caption.Visibility = Visibility.Collapsed;
                        if (_popup.IsOpen)
                            _popup.IsOpen = false;
                    }
                    */
                }
            }
        }

        public int ActualHeight
        {
            get
            {
                return (int)(Height);// / App.DeviceScale);
            }
        }
        public int ActualWidth
        {
            get
            {
                return (int)(Width);// / App.DeviceScale);
            }
        }
        #endregion

        #region Constructor
        public DicePanel()
        {
            OnTouchesBegan += DieClicked;
            _platform = Xamarin.Forms.DependencyService.Get<IImageService>().GetPlatform();
            //PanelStyle = DiceStyle.dpsBlue;
             //_popup.Child = _selectionPanel;
             //_selectionPanel.Tag = _popup;
             //_popup.Closed += _popup_Closed;
            /*for (int i = 0; i < 6;i++ )
            {
                _selectionPanel.AddDice(new Die(this) { Result = i + 1 });
            }*/
           
        }
        #endregion

        #region Methods
        void _popup_Closed(object sender, object e)
        {
            int oldvalue = _lastClickedDie.Result;
            //_lastClickedDie.Result = _selectionPanel.SelectedDice.Result;
            _lastClickedDie.DrawDie();
            ManualSetMode = false;
            if (DieChangedManual != null)
                DieChangedManual(_lastClickedDie.Frozen, oldvalue, _lastClickedDie.Result);
        }

        public void ChangeDice(int oldValue, int newValue)
        {
            var diceToChange = aDice.FirstOrDefault(f => f.Result == oldValue);
            if (diceToChange == null)
                return;
            diceToChange.Result = newValue;
            diceToChange.DrawDie();
        }

        /// <summary>
        /// Try to clear everything
        /// </summary>
        public void Dispose()
        {

            OnTouchesBegan -= DieClicked;
            //_popup.Closed -= _popup_Closed;

            foreach(var d in aDice)
            {
                d.Dispose();
                
            }
            //_selectionPanel.Dispose();
            aDice.Clear();
            DieFrameImages.Clear();
        }
        
        public async Task SetStyleAsync(DiceStyle value)
        {
                FStyle = value;

                switch (FStyle)
                {
                    case DiceStyle.dpsClassic:
                        if (DieAngle < 2) DieAngle = 0;
                        strStyle = "0/";
                        break;
                    case DiceStyle.dpsBrutalRed:
                        if (DieAngle < 2) DieAngle = 1;
                        strStyle = "1/";
                        break;
                    case DiceStyle.dpsBlue:
                        if (DieAngle < 2) DieAngle = 1;
                        strStyle = "2/";
                        break;
                }
            //loading image frames
            await LoadFrameImagesAsync("xrot.");
            await LoadFrameImagesAsync("yrot.");
            await LoadFrameImagesAsync("stop.");


            //dpBackcolor = Colors.Transparent;
            //this.Background = new SolidColorBrush(dpBackcolor);
            foreach (Die d in aDice)
            {
                d.DrawDie();
            }
            Invalidate();
        }
        private async Task LoadFrameImagesAsync(string rot)
        {
            if (PanelIsBusy != null)
                PanelIsBusy(true);
            for (int i = 0; i <= 35; i++)
            {
                string sPath = strImageRoot + strStyle + rot + i.ToString() + ".png";
                if (!DieFrameImages.ContainsKey(sPath))
                {
                    IImage img;
                    if (Xamarin.Forms.Device.OS == Xamarin.Forms.TargetPlatform.Android)
                    {
                        Stream stream = await Xamarin.Forms.DependencyService.Get<IImageService>().GetFileStream(sPath);
                        img = _platform.LoadImage(stream);
                    }
                    else
                    {
                        img = _platform.LoadImage(sPath);
                    }
                    DieFrameImages.Add(sPath, img);
                }
            }
            if (PanelIsBusy != null)
                PanelIsBusy(false);
        }
        
        private void GenerateDice()
        {
            Die d = null;
            Die dOld = null;
            bool bDone = false;
            int iTry = 0;

            //prepare caption
            //caption.Foreground = Brushes.SolidSanetBlue;


            aDice = new List<Die>();
            //this.Children.Clear();
            
            while (aDice.Count < NumDice)
            {
                d = new Die(this);
                iTry = 0;
                
                do
                {
                    iTry += 1;
                    bDone = true;
                    d.InitializeLocation();
                    foreach (Die dOld_loopVariable in aDice)
                    {
                        dOld = dOld_loopVariable;
                        if (d.Overlapping(dOld))
                        {
                            bDone = false;
                        }
                    }
                } while (!(bDone | iTry > 1000));

                aDice.Add(d);
                
                d.DrawDie();
                //this.Children.Add(d.PNG);
                

            }
            //this.Children.Add(caption);
            Invalidate();
        }
        
        public bool RollDice(List<int> aResults )
        {
            //LogManager.Log(LogLevel.Message, "DicePanel.RollDice",
            //    "Rolling...");

            if (ManualSetMode)
                ManualSetMode = false;
            //if (isRolling) return;


            //don't roll if all frozen
            if (AllDiceFrozen())
            {
                //LogManager.Log(LogLevel.Message, "DicePanel.RollDice","Can't roll... allfixed");
                return false;
            }
            if (BeginRoll != null)
            {
                BeginRoll();
            }
            //first values for fixed dices
            int j = aDice.Count(f=>f.Frozen);

            for (int i = 0; i <= aDice.Count - 1; i++)
            {
                int iResult = 0;
                var d = aDice[i];
                if (d.Frozen)
                    continue;
                if ((aResults != null))
                {
                    if (aResults.Count == aDice.Count)
                    {
                        iResult = aResults[j];
                        j += 1;
                    }

                }
                d.iSound = FRand.Next(1, 10);
                d.InitializeRoll(iResult);

            }
            
            sbLoop = new Storyboard();
            sbLoop.Duration = TimeSpan.FromMilliseconds(RollDelay);


            sbLoop.Completed += loop_Completed;

            // Start playing the Storyboard loop

            BeginLoop();
            return true;
        }
        
        
        private void BeginLoop()
        {
            if (WithSound)
            {
                foreach (Die d in aDice)
                {

                    if (d.iSound > 9)
                    {
                        d.PlaySound();
                        d.iSound = 1;
                    }
                    else
                    {
                        d.iSound += 1;
                    }

                }
            }
            sbLoop.Begin();
        }

        private void loop_Completed(object sender, EventArgs e)
        {
            foreach (Die d in aDice)
            {
                d.UpdateDiePosition();
                d.DrawDie();
                
            }

            //System.Threading.Thread.Sleep(RollDelay);

            HandleCollisions();
            //Me.Invalidate()
            //System.Windows.Forms.Application.DoEvents()
            if (!AllDiceStopped)
            {
                BeginLoop();
            }
            else
            {
                sbLoop.Completed -= loop_Completed;
                sbLoop = null;
                if (EndRoll != null)
                {
                    EndRoll();
                }
                
            }
            Invalidate();
        }

        private void HandleCollisions()
        {
            Die di = null;
            Die dj = null;
            int i = 0;
            int j = 0;

            if (NumDice == 1)
                return;

            //can't use foreach loops here, want to start j loop index AFTER first loop
            for (i = 0; i <= aDice.Count - 2; i++)
            {
                for (j = i + 1; j <= aDice.Count - 1; j++)
                {
                    di = aDice[i];
                    dj = aDice[j];
                    di.HandleCollision(dj);
                }
            }

        }

        /*private void canvasBackground_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RectangleGeometry rect = new RectangleGeometry();
            rect.Rect = new Rect(0, 0, this.Width, this.Height);
            this.Clip = rect;
            

        }*/
                     
        public void DieClicked(object sender, IEnumerable<Point> e)
        {
            Point pointClicked = e.First();
            //determine if die was clicked
            _lastClickedDie = null;
            foreach (Die d in aDice)
            {
                if (d.ClickedOn(pointClicked.X, pointClicked.Y))
                {
                    _lastClickedDie = d;
                    break; // TODO: might not be correct. Was : Exit For
                }
            }
            //no die was clicked
            if (_lastClickedDie == null)
                return;

            if (ManualSetMode)
            {
                /*_selectionPanel.SelectedDice = _lastClickedDie;
                _selectionPanel.Draw();
                _popup.IsOpen = true;*/
            }
            else if (ClickToFreeze)
            {
                _lastClickedDie.Frozen = !_lastClickedDie.Frozen;
                _lastClickedDie.DrawDie();
                if (DieFrozen != null)
                {
                    DieFrozen(_lastClickedDie.Frozen, _lastClickedDie.Result);
                }
                Invalidate();
            }
        }

        private bool DiceGenerated()
        {
            return (aDice != null);
        }

        public void OnDieBounced()
        {
            if (DieBounced != null)
            {
                DieBounced();
            }
        }

        public int FrozenCount()
        {
            int num = 0;
            foreach (Die d in aDice)
                if (d.Frozen) num++;
            return num;
        }

        //don't roll if all frozen
        public bool AllDiceFrozen()
        {

            
            foreach (Die d in aDice)
            {
                if (!d.Frozen)
                {
                    return false;
                }
            }
            return true;


        }
        public void FixDice(int index, bool isfixed)
        {
            foreach (Die d in aDice)
            {
                if (d.Result == index && d.Frozen==!isfixed)
                {
                    d.Frozen = isfixed;
                    d.DrawDie();
                    return;
                }
            }
        }

        public void ClearFreeze()
        {
            Die d = null;
            //If ClickToFreeze Then
            foreach (Die d_loopVariable in aDice)
            {
                d = d_loopVariable;
                if (d.Frozen)
                {
                    d.Frozen = false;
                    d.DrawDie();
                }
            }
            //End If
            Invalidate();
        }

        public override void Draw(ICanvas canvas, Rect rect)
        {
            base.Draw(canvas, rect);

            if (aDice!=null && aDice.Any())
            {
                foreach (var d in aDice)
                {
                    var r = new NGraphics.Rect(d.RectScaled.X, d.RectScaled.Y, d.RectScaled.Width, d.RectScaled.Height);
                    canvas.DrawImage(d.PNG, r,d.Opacity);
                }
            }
        }
        #endregion
    }





}
