using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remotis.Service
{
    public enum MessageType : short
    {
        Unknown=-1,
        Error=120,
        Warning=110,
        Information=70,
        PreValidate=10,
        PostValidate=20,
        PreExecute=30,
        PostExecute=40,
        Progress=60,
        StatusChange=50,
        QueryCancel=100,
        TaskFailed=130,
        Diagnostic=90,
        Custom=200,
        DiagnosticEx=140,
        NonDiagnostic=400,
        VariableValueChanged=80
    }
}
