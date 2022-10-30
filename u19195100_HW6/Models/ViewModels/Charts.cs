using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace u19195100_HW6.Models.ViewModels
{
    [DataContract]
    public class Charts
    {
		public Charts(string label, double y)
		{
			this.Label = label;
			this.Y = y;
		}

		
		[DataMember(Name = "label")]
		public string Label = "";

		
		[DataMember(Name = "y")]
		public Nullable<double> Y = null;
	}
}