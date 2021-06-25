using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Infineon.Yoda;
using TIBCO.Rendezvous;

namespace ServiceGroup
{
    class Program
    {
        /// <summary>
        /// The single instance of IfxTransport to be used on the application
        /// </summary>
        private static IfxTransport trp = new IfxTransport();

        /// <summary>
        /// IfxDoc that will hold the configuration.
        /// </summary>
        private static IfxDoc config = new IfxDoc();
        private static string Status;
        static void Main(string[] args)
        {
            try
            {
                // load configuration from the xml file
                config.Load(@"yodaconfiguration.xml");

                // initialize IfxTransport instance
                trp.Create(config);

                // We will listen for messages using service group subscription.
                // We can then use tibrvsend utility from TIBCO to send messages to our application
                // verify that the subscription was successfull.
                // IMPORTANT: You should always have at least three instances when running in service group mode.
                // This will avoid having the scheduler to assume the role of a worker.

                // subscribe to the desired subject using service group messaging. The second param corresponds to the unique
                // service group name.
                
                trp.AddToFaultToleranceGroup("WUX_T.APC.TEST","FTTESTZM", 1, IfxConst.IfxSubscribeMode.Normal);
                // set the event handler for incoming service group messages ( trp_ReliableMessage will handle incoming messages)
                
                trp.FTReliableMessage += new FTReliableMessageEventHandler(trp_ServiceGroupMessage);
                trp.FaultToleranceEvent += new FTEventHandler(Trp_FaultToleranceEvent);
                
                // wait for readline (should be done in a nicer way :) )
                Console.ReadLine();
            }
            catch (IfxException ex)
            {
                // always handle specific exceptions
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                // generic exception
                Console.WriteLine(ex.Message);
            }
            finally
            {

                // some extra cleaning here if needed
                Console.Read();
            }

        }

        

        private static void Trp_FaultToleranceEvent(IfxConst.IfxFTEvent status)
        {
            Status = status.ToString();
            Console.WriteLine("Status: " + Status);
            //throw new NotImplementedException();
        }

        static void trp_ServiceGroupMessage(string subject, string replySubject, IfxEnvelope envelope)
        {
            try
            {
                //Console.WriteLine("GetServiceGroupStatus: " + trp.GetServiceGroupStatus("FTTESTZM"));
                //Console.WriteLine("GetServiceGroupMemberRoleAsString: " + trp.GetServiceGroupMemberRoleAsString("FTTESTZM"));
                //Console.WriteLine("GetServiceGroupMemberRole: " + trp.GetServiceGroupMemberRole("FTTESTZM"));
                //Console.WriteLine("IsServiceGroupMemberRoleMonitoringEnabled: " + trp.IsServiceGroupMemberRoleMonitoringEnabled("FTTESTZM"));
                // extract document and print to output
                Console.WriteLine("I am active one");
                Console.WriteLine("Status: " + Status);
                IfxDoc doc = envelope.ExtractDocument();
                Console.WriteLine(DateTime.Now + doc.GetAsString());
            }
            catch (IfxException ex)
            {
                // always handle specific exceptions
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                // generic exception
                Console.WriteLine(ex.Message);
            }
            finally
            {
                // some extra cleaning here if needed
            }
        }

    }
}
