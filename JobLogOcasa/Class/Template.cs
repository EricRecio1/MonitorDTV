using System;
using System.Reflection;

namespace JobLogOcasa.Class
{
    public class Template
    {
        public Assembly Asembly { get; set; }
        public string FullName { get; set; }
        public string ClassName { get; set; }
        public string MethoName { get; set; }

        public Type MethoReturnType { get; set; }
        public int Type { get; set; }

        public ParameterInfo[] Params { get; set; }
        public string FileName { get; set; }
        public long ApplicationId { get; set; }
        public object[] ApplicationParamenters { get; set; }
    }
}
