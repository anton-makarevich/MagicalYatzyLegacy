﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Devices.Sensors;

namespace Sanet.DiceRoller.Models
{
    public class AccelerometerSensorWithShakeDetection : IDisposable
    {
        private const double ShakeThreshold = 0.2;
        private readonly Accelerometer _sensor = new Accelerometer();
        private AccelerometerReading _lastReading;
        private int _shakeCount;
        private bool _shaking;

        public AccelerometerSensorWithShakeDetection()
        {
            var sensor = new Accelerometer();
            if (sensor.State == SensorState.NotSupported)
                throw new NotSupportedException("Accelerometer not supported on this device");
            _sensor = sensor;
            _sensor.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<AccelerometerReading>>(_sensor_CurrentValueChanged);
        }

        

        public SensorState State
        {
            get { return _sensor.State; }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (_sensor != null)
                _sensor.Dispose();
        }

        #endregion

        public event EventHandler ShakeDetectedHandler;

        public event EventHandler ShakeDetected
        {
            add
            {
                ShakeDetectedHandler += value;

            }
            remove
            {
                ShakeDetectedHandler -= value;
                _sensor.CurrentValueChanged -= _sensor_CurrentValueChanged;
            }
        }

        public void Start()
        {
            if (_sensor != null)
                _sensor.Start();
        }

        public void Stop()
        {
            if (_sensor != null)
                _sensor.Stop();
        }
        void _sensor_CurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e)
        {
            //Code for checking shake detection
            if (_sensor.State == SensorState.Ready)
            {
                AccelerometerReading reading = e.SensorReading;
                try
                {
                    if (_lastReading.Acceleration != null)
                    {
                        if (!_shaking && CheckForShake(_lastReading, reading, ShakeThreshold) && _shakeCount >= 1)
                        {
                            //We are shaking
                            _shaking = true;
                            _shakeCount = 0;
                            OnShakeDetected();
                        }
                        else if (CheckForShake(_lastReading, reading, ShakeThreshold))
                        {
                            _shakeCount++;
                        }
                        else if (!CheckForShake(_lastReading, reading, 0.2))
                        {
                            _shakeCount = 0;
                            _shaking = false;
                        }
                    }
                    _lastReading = reading;
                }
                catch (Exception ex)
                {
                    var s = ex.Message;
                }
            }
        }
        

        private void OnShakeDetected()
        {
            if (ShakeDetectedHandler != null)
                ShakeDetectedHandler(this, EventArgs.Empty);
        }

        private static bool CheckForShake(AccelerometerReading last, AccelerometerReading current,
                                            double threshold)
        {
            double deltaX = Math.Abs((last.Acceleration.X - current.Acceleration.X));
            double deltaY = Math.Abs((last.Acceleration.Y - current.Acceleration.Y));
            double deltaZ = Math.Abs((last.Acceleration.Z - current.Acceleration.Z));

            return (deltaX > threshold && deltaY > threshold) ||
                    (deltaX > threshold && deltaZ > threshold) ||
                    (deltaY > threshold && deltaZ > threshold);
        }
    }
}
