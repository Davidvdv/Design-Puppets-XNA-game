using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPuppets
{
	/// <summary>
	/// Interface die zorgt dat de behaviours van de puppet allemaal de method DoTask hebben.
	/// </summary>
	public interface IPuppetBehaviour
	{
		void DoTask(Puppet puppet);
	}
}
