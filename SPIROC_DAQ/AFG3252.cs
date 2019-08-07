using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NationalInstruments.VisaNS;

namespace SPIROC_DAQ
{
    class AFG3252
    {
        private MessageBasedSession session;
        public bool initial()
        {
            try
            {
                var rsrc = ResourceManager.GetLocalManager().FindResources(settings.AFG_DESCR3252);
                rsrc = ResourceManager.GetLocalManager().FindResources(settings.AFG_DESCR3252);
                if (rsrc != null)
                {
                    session = (MessageBasedSession)ResourceManager.GetLocalManager().Open(rsrc[0]);
                }              
                
            }
            catch (InvalidCastException)
            {
                MessageBox.Show("Resource selected must be a message-based session");
            }
            catch (Exception exp)
            {
               // MessageBox.Show(exp.Message);
            }
            
            return (session != null);
        }

        public void Close()
        {
            session.Dispose();
        }

        public bool isConnected()
        {
            return (session != null);
        }
        public void setVoltage(int chn,int mV)
        {
            // low level is 0, set high level
            try
            {
                session.Write("SOURce" + chn.ToString() + ":VOLTage:LEVel:IMMediate:LOW " + mV + "mV");
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }

        }

        public void setOffset(int chn,int mV)
        {
            try
            {
                session.Write("SOURce" + chn.ToString() + ":VOLTage:LEVel:IMMediate:OFFSet " + mV + "mV");
            }
            catch(Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        public void closeOutput()
        {
            try
            {
                session.Write("OUTPut1:STATe OFF");
                session.Write("OUTPut2:STATe OFF");
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        public void closeOutput(int chn)
        {
            try
            {
                session.Write("OUTPut"+chn.ToString()+":STATe OFF");
               
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        public void openOutput()
        {
            try
            {
                session.Write("OUTPut1:STATe ON");
                session.Write("OUTPut2:STATe ON");
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        public void openOutput(int chn)
        {
            try
            {
                session.Write("OUTPut" + chn.ToString() + ":STATe ON");

            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        public void delaySet(int channel, int value, int offset=0)
        {
            string cmd;
            try
            {
                //cmd = string.Format("SOURce{0}:BURSt:TDELay {1}ns", channel, value);
                cmd = string.Format("SOURce{0}:BURSt:TDELay {1}ns", channel, value + offset);
                session.Write(cmd);
            }
            catch(Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }
        // core operation
        public void Write(string cmd)
        {
            session.Write(cmd);
        }
    }
}
