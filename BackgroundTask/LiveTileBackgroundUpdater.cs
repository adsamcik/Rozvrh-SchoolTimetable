using System;
using Windows.ApplicationModel.Background;
using SharedLib;

namespace BackgroundTasks {
    public sealed class LiveTileBackgroundUpdater : IBackgroundTask {
        public void Run(IBackgroundTaskInstance taskInstance) {
            Data.Initialize();
            NotificationManager.PrepareLiveTile();
        }
    }
}
