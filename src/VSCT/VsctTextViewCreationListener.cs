﻿using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;

namespace MadsKristensen.ExtensibilityTools.Vsct
{
    [Export(typeof(IVsTextViewCreationListener))]
    [ContentType(VsctContentTypeDefinition.VsctContentType)]
    [TextViewRole(PredefinedTextViewRoles.Interactive)]
    internal sealed class VsctTextViewCreationListener : IVsTextViewCreationListener
    {
        [Import]
        IVsEditorAdaptersFactoryService AdaptersFactory = null;

        [Import]
        ICompletionBroker CompletionBroker = null;

        [Import]
        internal SVsServiceProvider ServiceProvider = null;

        public void VsTextViewCreated(IVsTextView textViewAdapter)
        {
            IWpfTextView view = AdaptersFactory.GetWpfTextView(textViewAdapter);

            //view.TextBuffer.Properties.GetOrCreateSingletonProperty(() => view);

            VsctCompletionController completion = new VsctCompletionController(view, CompletionBroker);
            IOleCommandTarget completionNext;
            textViewAdapter.AddCommandFilter(completion, out completionNext);
            completion.Next = completionNext;
        }
    }
}
