﻿using BioBotCommunication.Serial.Movement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BioBotCommunication.Serial.Utils.Serial
{
    public class SerialConsumer : IConsumer
    {
        private readonly Billboard billboard;
        private readonly String waitValue;
	    private volatile bool isStopped = false;
        private bool isNotified = false;
        private Object isNotifiedLock;
        private Thread consumerThread;
        public event EventHandler<SerialConsumerCompletionEventargs> onCompletion;

        public SerialConsumer(Billboard billboard, String waitValue)
        {
            isNotifiedLock = new object();
            this.billboard = billboard;
            this.billboard.register(this);
            this.waitValue = waitValue;
            consumerThread = new Thread(run);
        }

        public void start()
        {
            consumerThread.Start();
        }

        public void notifyValue(bool value)
        {
            lock (isNotifiedLock)
            {
                isNotified = value;
            }
        }
        public bool isNotifiedValue()
        {
            lock (isNotifiedLock)
            {
                return isNotified;
            }
        }

        public void stopThread()
        {
            isStopped = true;
            consumerThread.Join(1000);
            if (consumerThread.IsAlive)
            {
                KillTheThread();
            }
        }

        public void run()
        {
            while (!isStopped)
            {
                Thread.Sleep(100);

                if (isNotifiedValue())
                {
                    List<Object> listValues = billboard.getValues();
                    foreach(Object value in listValues)
                    {
                        if(value is String)
                        {
                            String valueString = value as String;
                            if (valueString.Contains(waitValue))
                            {
                                Console.WriteLine("Consume: " + waitValue);
                                billboard.delete(value, this);
                                isStopped = true;
                                break;
                            }
                        }
                        
                    }
                    notifyValue(false);
                }
            }
            if(onCompletion != null)
            {
                onCompletion(this, new SerialConsumerCompletionEventargs());
                billboard.unregister(this);
            }
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, ControlThread = true)]
        private void KillTheThread()
        {
            consumerThread.Abort();
        }

        public bool objectEquals(object value, object compare)
        {
            String valueString = value as String;
            String compareString = compare as String;
            if (valueString == null || compareString == null) return false;
            return valueString.Contains(compareString);
        }
    }
}