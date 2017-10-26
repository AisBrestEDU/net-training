using System.Xml.Serialization;
namespace Serialization.Tasks
{
    // TODO: Generate InstrumentationManifest class from Serialization.Tests\Resources\eventman.xsd to make it ready for XML serialization
    // Use XSD.EXE tool to generate class source code (or die trying to do it manually)
    // Please make separate properties for Instrumentation, Localization and Metadata sections of xml.
    // Serialization.Tests\Resources\CLR-ETW.man file will be used to test the deserialization


    public partial class InstrumentationManifest
    {

        private object[] itemsField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute()]
       // [System.Xml.Serialization.XmlElementAttribute("events", typeof(EventsType))]
        public object[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }
            set
            {
                this.anyAttrField = value;
            }
        }
    }

}


