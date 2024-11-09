using System.Collections.ObjectModel;
using AnimaFiltering.Services;

//using Avalonia.Xaml.Interactions.DragAndDrop;
using Avalonia.Input;
using Avalonia.Platform.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace AnimaFiltering.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<AttachmentLink> Attachments { get; } = new ObservableCollection<AttachmentLink>();

        public CameraManager Cameras { get; } = App.Services.GetRequiredService<CameraManager>();

        public void DropHandler(object sender, DragEventArgs e)
        {
            if (e.Data.GetFiles() is { } fileNames)
            {
                foreach (var file in fileNames)
                {
                    Attachments.Add(new AttachmentLink(file));
                }
            }
        }
    }

    public class AttachmentLink
    {
        public IStorageItem FilePath { get; }

        public AttachmentLink(IStorageItem filePath)
        {
            FilePath = filePath;
        }
    }
}