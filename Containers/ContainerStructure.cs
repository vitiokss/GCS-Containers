using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MissionPlanner.Containers
{   
    abstract class ContainerStructure
    {
        
        public List<ContainerObject> ContainerList = new List<ContainerObject>();

        public void AddContainerToList(ContainerObject c)
        {
            this.ContainerList.Add(c);
        }

    }
}
