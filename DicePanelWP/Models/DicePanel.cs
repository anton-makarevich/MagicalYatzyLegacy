using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Sanet.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.ComponentModel;

#if WinRT
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Sanet.AllWrite;

#else
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Resources;
#endif
namespace Sanet.Kniffel.DicePanel

{
    public enum dpStyle
    {
        dpsClassic = 0,
        dpsBrutalRed = 1,
        dpsBlue = 2
    }
    /// <summary>
    /// Dice Panel Control - where we are rolling dices
    /// </summary>
    public class DicePanel : Canvas
    {
        //popup to show dice value selection panel
        Popup _popup = new Popup();
        DiceValueSelectionPanel _selectionPanel = new DiceValueSelectionPanel();

        //To show messages
        TextBlock caption = new TextBlock();

        public Random FRand = new Random();
        public bool PlaySound { get; set; }

        public event DieBouncedEventHandler DieBounced;
        public delegate void DieBouncedEventHandler();
        public event DieFrozenEventHandler DieFrozen;
        public delegate void DieFrozenEventHandler(bool @fixed, int Value);
        public delegate void DieChangedEventHandler(bool isfixed, int oldvalue,int newvalue);
        public event DieChangedEventHandler DieChangedManual;
        private dpStyle FStyle = dpStyle.dpsClassic;
        public List<Die> aDice = new List<Die>();

        private Storyboard sbLoop;
        public event EndRollEventHandler EndRoll;
        public delegate void EndRollEventHandler();
        public event BeginRollEventHandler BeginRoll;
        public delegate void BeginRollEventHandler();

        public bool TreeDScale { get; set; }
        public double TreeDScaleCoef { get; set; }
        //cashed images
        public Dictionary<string, BitmapSource> DieFrameImages = new Dictionary<string, BitmapSource>();
#if WinRT
        string strImageRoot = "ms-appx:///Images/";
#else
        const string strImageRoot = "/Images/";
#endif
        public string strStyle;
        
        public delegate void StartLoadingEventHandler();
       
        public delegate void StopLoadingEventHandler();
        public DicePanel()
        {
                                    
#if WinRT
            Tapped += DieClicked;
#else
            MouseLeftButtonDown += DieClicked;
#endif
            //InitializeComponent()
            PanelStyle = dpStyle.dpsBlue;
             _popup.Child = _selectionPanel;
             _selectionPanel.Tag = _popup;
             _popup.Closed += _popup_Closed;
            for (int i = 0; i < 6;i++ )
            {
                _selectionPanel.AddDice(new Die(this) { Result = i + 1 });
            }
           
        }

        void _popup_Closed(object sender, object e)
        {
            int oldvalue = _lastClickedDie.Result;
            _lastClickedDie.Result = _selectionPanel.SelectedDice.Result;
            _lastClickedDie.DrawDie();
            ManualSetMode = false;
            if (DieChangedManual != null)
                DieChangedManual(_lastClickedDie.Frozen, oldvalue, _lastClickedDie.Result);
        }

        /// <summary>
        /// Try to clear everything
        /// </summary>
        public void Dispose()
        {
#if WinRT
            Tapped -= DieClicked;
            _popup.Closed -= _popup_Closed;
#else
            MouseLeftButtonDown -= DieClicked;
            _popup.Closed -= _popup_Closed;
#endif
            _selectionPanel.Dispose();
            
        }
        
        private void LoadFrameImages(string rot)
        {
            for (int i = 0; i <= 35; i++)
            {
                string sPath = strImageRoot + strStyle + rot + i.ToString() + ".png";
                if (!DieFrameImages.ContainsKey(sPath.Replace("ms-appx://","")))
                {
                    Uri ur = new Uri(sPath, UriKind.RelativeOrAbsolute);
                    BitmapSource img = new BitmapImage(ur);
#if WinRT
                    sPath = ur.AbsolutePath;
#endif
                    DieFrameImages.Add(sPath, img);
                }
            }
        }

        private Color dpBackcolor = Colors.LightGray;
        public dpStyle PanelStyle
        {
            get { return FStyle; }
            set
            {
                //If FStyle = value And DieFrameImages.Values.Count > 0 Then Exit Property
                FStyle = value;

                switch (FStyle)
                {
                    case dpStyle.dpsClassic:
                        if (DieAngle<2)DieAngle = 0;
                        //dpBackcolor = Colors.Green
                        strStyle = "0/";
                        break;
                    case dpStyle.dpsBrutalRed:
                        if (DieAngle < 2) DieAngle = 1;
                        //dpBackcolor = Colors.Black
                        strStyle = "1/";
                        break;
                    case dpStyle.dpsBlue:
                        if (DieAngle < 2) DieAngle = 1;
                        //dpBackcolor = Colors.Transparent 'Color.FromArgb(100, )
                        strStyle = "2/";
                        break;
                }
                //loading image frames
                LoadFrameImages("xrot.");
                LoadFrameImages("yrot.");
                LoadFrameImages("stop.");

                //precashing image frames
                foreach (BitmapSource b in DieFrameImages.Values)
                {
                    Image img = new Image();
                    img.Source = b;
                    base.Children.Add(img);
                }
                base.Children.Clear();

                dpBackcolor = Colors.Transparent;
                this.Background = new SolidColorBrush(dpBackcolor);
                foreach (Die d in aDice)
                {
                    d.DrawDie();
                    this.Children.Add(d.PNG);
                }
                //bckim '

            }
        }
        private int FDieAngle = 0;
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
            }
        }
        private int FMaxRollLoop = 75;
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



        private int FNumDice = 2;

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


        private bool FDebugDrawMode = false;
        
        /// <summary>
        /// Draws a Box Around the Die for Collision Debugging
        /// </summary>
        public bool DebugDrawMode
        {
            get { return FDebugDrawMode; }
            set { FDebugDrawMode = value; }
        }

        //new

        private bool FClickToFreeze;
        //new
       
        /// <summary>
        /// Allows user to click dice to lock their movement
        /// </summary>
        public bool ClickToFreeze
        {
            get { return FClickToFreeze; }
            set { FClickToFreeze = value; }
        }


        private void GenerateDice()
        {
            Die d = null;
            Die dOld = null;
            bool bDone = false;
            int iTry = 0;

            //prepare caption
            caption.Foreground = Brushes.SolidSanetBlue;
            caption.FontSize = 28;
            caption.SetValue(Canvas.LeftProperty, Convert.ToDouble(15));
            caption.SetValue(Canvas.TopProperty, Convert.ToDouble(15));

            aDice = new List<Die>();
            this.Children.Clear();
            //MC.Children.Clear()

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
                this.Children.Add(d.PNG);
                

            }
            this.Children.Add(caption);
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
                return new DieResult{ DiceResults=dr};
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
        
        public void RollDice(List<int> aResults )
        {
            if (ManualSetMode)
                ManualSetMode = false;
            //if (isRolling) return;
            Die d = null;


            //don't roll if all frozen
            if (AllDiceFrozen())
                return;
            if (BeginRoll != null)
            {
                BeginRoll();
            }
            //first values for fixed dices
            int j = aDice.Count(f=>f.Frozen);

            for (int i = 0; i <= aDice.Count - 1; i++)
            {
                int iResult = 0;
                d = aDice[i];
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

                d.InitializeRoll(iResult);

            }
            
            sbLoop = new Storyboard();
            sbLoop.Duration = TimeSpan.FromMilliseconds(RollDelay);


            sbLoop.Completed += loop_Completed;

            // Start playing the Storyboard loop

            BeginLoop();

        }
                
        private void BeginLoop()
        {
            
            foreach (Die d in aDice)
            {
                
                if (PlaySound)
                {
                    if (d.iSound >= 8)
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
#if WinRT
        private void loop_Completed(object sender, object e)
#else
        private void loop_Completed(object sender, EventArgs e)
#endif
        {
            foreach (Die d in aDice)
            {
                d.UpdateDiePosition();
                d.DrawDie();
                if (d.IsPlaying)
                    d.StopSound();
            }
#if WinRT
            new System.Threading.ManualResetEvent(false).WaitOne(RollDelay);
#else
            System.Threading.Thread.Sleep(RollDelay);
#endif
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


        }

        private int _RollDelay;
        public int RollDelay
        {
            get { return _RollDelay; }
            set { _RollDelay = value; }
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

        private void canvasBackground_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RectangleGeometry rect = new RectangleGeometry();
            rect.Rect = new Rect(0, 0, this.ActualWidth, this.ActualHeight);
            this.Clip = rect;


        }

        Die _lastClickedDie;
        //new
#if WinRT
        public void DieClicked(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
#else
        public void DieClicked(object sender, MouseButtonEventArgs e)
#endif
        {
            Point pointClicked = e.GetPosition(this);
            //determine if die was clicked
            foreach (Die d in aDice)
            {
                if (d.ClickedOn(pointClicked.X, pointClicked.Y))
                {
                    _lastClickedDie = d;
                    break; // TODO: might not be correct. Was : Exit For
                }
            }
            if (ManualSetMode)
            {
                _selectionPanel.SelectedDice = _lastClickedDie;
                _selectionPanel.Draw();
                _popup.IsOpen = true;
            }
            else if (ClickToFreeze)
            {
                _lastClickedDie.Frozen = !_lastClickedDie.Frozen;
                _lastClickedDie.DrawDie();
                if (DieFrozen != null)
                {
                    DieFrozen(_lastClickedDie.Frozen, _lastClickedDie.Result);
                }

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

        //new
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
        public void FixDice(int index)
        {
            foreach (Die d in aDice)
            {
                if (d.Result == index && !d.Frozen)
                {
                    d.Frozen = true;
                    return;
                }
            }
        }

        //new
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

        }

        
        private bool _ManualSetMode=false;
        public bool ManualSetMode
        {
            get { return _ManualSetMode; }
            set
            {
                if (_ManualSetMode != value)
                {
                    _ManualSetMode = value;
                    if (value)
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
                    
                }
            }
        }

    }
    /// <summary>
    /// Dice oject
    /// </summary>
    public class Die : UserControl
    {

        private const int MAXMOVE = 5;

        public MediaElement mSound;
        private enum DieStatus
        {
            dsStopped = 0,
            dsRolling = 1,
            dsLanding = 2
        }
        private Image _png;
        public Image PNG
        {
            get { return _png; }
            set { _png = value; }
        }

        private int FRollLoop;
        private int h = 72;
        private int w = 72;
        private int FxPos;

        private int FyPos;
        private int dxDir;
        //направление движения
        private int dyDir;

        public DicePanel FPanel;

        private DieStatus FStatus = DieStatus.dsLanding;
        public string StyleString
        {
            get { return "/Images/" + FPanel.strStyle; }
        }
        private string strrot;

        //private string strframe;

        private BitmapSource GetFramePic()
        {
            //If bA.Length = 0 Then Return Nothing
            string sPath = StyleString + strrot + Frame.ToString() + ".png";

            return FPanel.DieFrameImages[sPath];

        }
        public Die(DicePanel pn)
            : base()
        {
            PNG = new Image();
            PNG.CacheMode = new BitmapCache();
            //Style = pn.Style
            FPanel = pn;
            //mSound.AutoPlay = False
            pStatus = DieStatus.dsStopped;
        }
        public bool IsPlaying;
        public void PlaySound()
        {
            mSound = new MediaElement();
#if WINDOWS_PHONE
            StreamResourceInfo sri = Application.GetResourceStream(new Uri("/KiniffelOnline;component/diceroll1.mp3", UriKind.RelativeOrAbsolute));
            mSound.SetSource(sri.Stream);
#endif
            IsPlaying = true;
        }
        public void StopSound()
        {
            IsPlaying = false;
            mSound.Stop();
            mSound = null;
        }

        private int FFrame;
        private int Frame
        {
            get { return FFrame; }
            set
            {
                FFrame = value;

                if (FFrame < 0)
                    FFrame += 36;
                if (FFrame > 35)
                    FFrame -= 36;
            }
        }


        //результат кости 1-6
        private int FResult = 1;
        public int Result
        {
            get { return FResult; }
            set
            {
                if (value < 1 | value > 6)
                {
                    throw new Exception("Неправильное значение кости");
                }
                else
                {
                    FResult = value;
                }
            }
        }

        private int xPos
        {
            get { return FxPos; }
            set
            {
                int ks = 0;
                //псевдо перспектива - сужение стола на верху
                //If FPanel.TreeDScale Then
                //    Dim ks1 = FPanel.TreeDScaleCoef / 2 * FPanel.ActualWidth
                //    Dim M As Integer = FPanel.ActualHeight - w
                //    ks = (M - yPos) * ks1 / M
                //    ' MessageBox.Show(FPanel.TreeDScaleCoef & ",  " & ks1)
                //End If

                FxPos = value;

                if (FxPos < 0 + ks)
                {
                    FxPos = 0 + ks;
                    BounceX();
                }
                if (FxPos > Convert.ToInt32(FPanel.ActualWidth) - w - ks)
                {
                    FxPos = Convert.ToInt32(FPanel.ActualWidth) - w - ks;
                    BounceX();
                }
            }
        }

        private int yPos
        {
            get { return FyPos; }
            set
            {
                FyPos = value;

                if (FyPos < 0)
                {
                    FyPos = 0;
                    BounceY();
                }
                if (FyPos > FPanel.ActualHeight - h)
                {
                    FyPos = Convert.ToInt32(FPanel.ActualHeight) - h;
                    BounceY();
                }
            }
        }


        private bool FFrozen;
        public bool Frozen
        {
            get { return FFrozen; }

            set { FFrozen = value; }
        }

        public void InitializeLocation()
        {
            try
            {
                xPos = FPanel.FRand.Next(0, Convert.ToInt32(FPanel.ActualWidth) - w);
                yPos = FPanel.FRand.Next(0, Convert.ToInt32(FPanel.ActualHeight) - h);
            }
            catch 
            {
                xPos = 0;
                yPos = 0;
            }
            UpdatePngPosition();
        }
        public int iSound;

        public void UpdateDiePosition()
        {
            switch (pStatus)
            {
                case DieStatus.dsLanding:

                    Frame -= 1;
                    break;
                case DieStatus.dsRolling:
                    //увеличиваем либо уменьшаем в зависимости отнаправления
                    Frame += (1 * Math.Sign(dyDir));
                    break;
                //mSound.Stop() 'Position = TimeSpan.Zero


                case DieStatus.dsStopped:
                    return;

                 
            }


            xPos += dxDir;
            yPos += dyDir;

            FRollLoop += 1;

            switch (pStatus)
            {
                case DieStatus.dsRolling:
                    //если достигнуто максимальное число вращений останавливаемся
                    if (FRollLoop > FPanel.MaxRollLoop & FPanel.FRand.Next(1, 100) < 10)
                    {
                        pStatus = DieStatus.dsLanding;
                        FRollLoop = 0;

                        Frame = Result * 6;
                    }

                    break;
                case DieStatus.dsLanding:

                    if (FRollLoop > (5 - FPanel.DieAngle))
                    {
                        pStatus = DieStatus.dsStopped;
                    }
                    break;
            }
            UpdatePngPosition();
        }
        private void UpdatePngPosition()
        {
            PNG.SetValue(Canvas.LeftProperty, Convert.ToDouble(xPos));
            PNG.SetValue(Canvas.TopProperty, Convert.ToDouble(yPos));

        }
        //new, done for directional changing, collision but
        private DieStatus pStatus
        {
            get { return FStatus; }
            set
            {
                FStatus = value;
                if (value == DieStatus.dsStopped)
                {
                    dxDir = 0;
                    //stop direction
                    dyDir = 0;

                }
            }
        }

        public void InitializeRoll(int iResult = 0)
        {
            if (iResult < 0 | iResult > 6)
                iResult = 0;
            //new
            if (!Frozen)
            {
                //скорость и направление начального движения
                do
                {
                    dxDir = FPanel.FRand.Next(-MAXMOVE, MAXMOVE + 1);
                } while (!(Math.Abs(dxDir) > 2));
                do
                {
                    dyDir = FPanel.FRand.Next(-MAXMOVE, MAXMOVE + 1);
                } while (!(Math.Abs(dyDir) > 2));
                if (iResult == 0)
                {
                    Result = FPanel.FRand.Next(1, 7);
                    //decide what the result will be
                }
                else
                {
                    Result = iResult;
                }
                FRollLoop = 0;
                pStatus = DieStatus.dsRolling;
            }
            else
            {
                pStatus = DieStatus.dsStopped;
            }
        }
        //Private _style As dpStyle = dpStyle.dpsClassic
        //Public Property Style As dpStyle
        //    Get
        //        Return _style
        //    End Get
        //    Set(ByVal value As dpStyle)
        //        _style = value
        //    End Set
        //End Property
        public void DrawDie()
        {
            //RESCALE
            if (FPanel.TreeDScale)
            {
                var M = FPanel.ActualHeight - 72;
                double k = this.yPos / M * FPanel.TreeDScaleCoef + (1 - FPanel.TreeDScaleCoef);
                this.h = Convert.ToInt32(72 * k);
                this.w = Convert.ToInt32(72 * k);
                ScaleTransform st = new ScaleTransform();
                st.ScaleX = k;
                st.ScaleY = k;
                st.CenterX = PNG.Width / 2;
                st.CenterY = PNG.Height / 2;
                PNG.RenderTransform = st;
            }
            //select the correct bitmap based on what the die is doing, and what direction it's going
            if (pStatus == DieStatus.dsRolling)
            {
                PNG.Opacity = 1;
                if ((dxDir * dyDir) > 0)
                {
                    strrot = "yrot.";
                    PNG.Source = GetFramePic();

                }
                else
                {
                    strrot = "xrot.";
                    PNG.Source = GetFramePic();
                }

            }
            else
            {
                Frame = (Result - 1) * 6 + FPanel.DieAngle;
                strrot = "stop.";
                PNG.Source = GetFramePic();
                if (Frozen)
                {
                    //strrot = "stop." '"halo."
                    PNG.Opacity = 0.5;
                }
                else
                {
                    PNG.Opacity = 1;
                }
            }


        }

        public bool IsNotRolling
        {
            get { return pStatus == DieStatus.dsStopped; }
        }

        public bool IsRolling
        {
            get { return pStatus == DieStatus.dsRolling; }
        }

        public Rect Rect
        {
            get { return new Rect(xPos, yPos, w, h); }
        }

        public bool Overlapping(Die d)
        {
            //Return d.Rect.Interse(Me.Rect)

            Rect rect1 = this.Rect;
            rect1.Intersect(d.Rect);
            return !(rect1 == Rect.Empty);
        }

        public void HandleCollision(Die d)
        {
            if (this.Overlapping(d))
            {
                if (Math.Abs(d.yPos - this.yPos) <= Math.Abs(d.xPos - this.xPos))
                {
                    HandleBounceX(d);
                }
                else
                {
                    HandleBounceY(d);
                }
            }
        }


        private void HandleBounceX(Die d)
        {
            Die dLeft = null;
            Die dRight = null;

            if (this.xPos < d.xPos)
            {
                dLeft = this;
                dRight = d;
            }
            else
            {
                dLeft = d;
                dRight = this;
            }

            //moving toward each other
            if (dLeft.dxDir >= 0 & dRight.dxDir <= 0)
            {
                this.BounceX();
                d.BounceX();
                return;
            }

            //moving right, left one caught up to right one
            if (dLeft.dxDir > 0 & dRight.dxDir >= 0)
            {
                dLeft.BounceX();
                return;
            }

            //moving left, right one caught up to left one
            if (dLeft.dxDir <= 0 & dRight.dxDir < 0)
            {
                dRight.BounceX();
            }

        }


        private void HandleBounceY(Die d)
        {
            Die dTop = null;
            Die dBot = null;

            if (this.yPos < d.yPos)
            {
                dTop = this;
                dBot = d;
            }
            else
            {
                dTop = d;
                dBot = this;
            }

            if (dTop.dyDir >= 0 & dBot.dyDir <= 0)
            {
                this.BounceY();
                d.BounceY();
                return;
            }

            //moving down, top one caught up to bottom one
            if (dTop.dyDir > 0 & dBot.dyDir >= 0)
            {
                dTop.BounceY();
                return;
            }

            //moving left, bottom one caught up to top one
            if (dTop.dyDir <= 0 & dBot.dyDir < 0)
            {
                dBot.BounceY();
            }

        }

        private void BounceX()
        {
            dxDir = -dxDir;

            //no sound if not moving
            if (pStatus != DieStatus.dsStopped)
            {
                FPanel.OnDieBounced();
            }
        }

        private void BounceY()
        {
            dyDir = -dyDir;

            //no sound if not moving
            if (pStatus != DieStatus.dsStopped)
            {
                FPanel.OnDieBounced();
            }
        }

        //new
        public bool ClickedOn(double x, double y)
        {
            return this.Rect.Contains(new Point(x, y));
        }

    }
    /// <summary>
    /// array of dice values with helpers
    /// </summary>
    public class DieResult
    {
        public List<int> DiceResults { get; set; }
        public int Total 
        {
            get
            {
                if (DiceResults != null)
                    return DiceResults.Sum();
                return 0;
            }
        }
        public int NumDice
        {
            get
            {
                if (DiceResults != null)
                    return DiceResults.Count;
                return 0;
            }
        }

        public int NumDiceOf(int value)
        {
            return DiceResults.Count(f => f == value);
        }
    }

    
        
}
