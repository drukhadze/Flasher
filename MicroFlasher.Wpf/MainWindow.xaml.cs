﻿using System.Windows;
using System.Windows.Input;
using MicroFlasher.Models;
using MicroFlasher.Views;
using MicroFlasher.Views.Operations;
using MicroFlasher.Views.SerialMonitor;
using Atmega.Hex;
using Microsoft.Win32;

namespace MicroFlasher {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private readonly FlasherModel _model = new FlasherModel();

        public FlasherModel Model { get { return _model; } }

        public MainWindow() {
            InitializeComponent();
            DataContext = _model;
        }

        private void OpenFlashCommand(object sender, ExecutedRoutedEventArgs e) {
            var dlg = new OpenFileDialog {
                Filter = "Intel Hex File|*.hex;*.eep|All Files|*.*"
            };

            var result = dlg.ShowDialog();

            if (result == true) {
                try {
                    _model.OpenFlash(dlg.FileName);
                } catch (HexFileException exc) {
                    MessageBox.Show(this, exc.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void OpenEepromCommand(object sender, ExecutedRoutedEventArgs e) {
            var dlg = new OpenFileDialog {
                Filter = "Intel Hex File|*.hex;*.eep|All Files|*.*"
            };

            var result = dlg.ShowDialog();

            if (result == true) {
                try {
                    _model.OpenEeprom(dlg.FileName);
                } catch (HexFileException exc) {
                    MessageBox.Show(this, exc.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SaveCommand(object sender, ExecutedRoutedEventArgs e) {
            var dlg = new SaveFileDialog {
                Filter = "Intel Hex File|*.hex;*.eep|All Files|*.*"
            };

            var result = dlg.ShowDialog();

            if (result == true) {
                _model.SaveFile(dlg.FileName);
            }
        }

        #region Read

        private void ReadDeviceCommand(object sender, ExecutedRoutedEventArgs e) {
            var dlg = new ReadDeviceWindow {
                DataContext = new FlasherOperationModel(_model),
                Owner = this
            };
            dlg.ShowDialog();
        }

        private void ReadFlashCommand(object sender, ExecutedRoutedEventArgs e) {
            var dlg = new ReadFlashWindow {
                DataContext = new FlasherOperationModel(_model),
                Owner = this
            };
            dlg.ShowDialog();
        }

        private void ReadEepromCommand(object sender, ExecutedRoutedEventArgs e) {
            var dlg = new ReadEepromWindow {
                DataContext = new FlasherOperationModel(_model),
                Owner = this
            };
            dlg.ShowDialog();
        }

        private void ReadLockBitsCommand(object sender, ExecutedRoutedEventArgs e) {
            var dlg = new ReadLockBitsWindow {
                DataContext = new FlasherOperationModel(_model),
                Owner = this
            };
            dlg.ShowDialog();
        }

        private void ReadFuseBitsCommand(object sender, ExecutedRoutedEventArgs e) {
            var dlg = new ReadFuseBitsWindow {
                DataContext = new FlasherOperationModel(_model),
                Owner = this
            };
            dlg.ShowDialog();
        }

        #endregion

        #region Write

        private void WriteDeviceCommand(object sender, ExecutedRoutedEventArgs e) {
            var msgResult = MessageBox.Show("Are you sure you want start writing to the device. All previous data will be lost", "Write confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (msgResult == MessageBoxResult.Yes) {
                if (_model.Config.AutoErase) {
                    var eraseDlg = new EraseDeviceWindow {
                        DataContext = new FlasherOperationModel(_model),
                        Owner = this
                    };
                    if (eraseDlg.ShowDialog() != true) return;
                }

                var writeDlg = new WriteDeviceWindow {
                    DataContext = new FlasherOperationModel(_model),
                    Owner = this
                };
                var writeResult = writeDlg.ShowDialog();
                if (writeResult == true && _model.Config.AutoVerify) {
                    VerifyDeviceCommand(this, null);
                }
            }
        }

        private void WriteFlashCommand(object sender, ExecutedRoutedEventArgs e) {
            var msgResult = MessageBox.Show("Are you sure you want start writing flash. Device may become unoperable", "Flash confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (msgResult == MessageBoxResult.Yes) {

                if (_model.Config.AutoErase) {
                    var eraseDlg = new EraseDeviceWindow {
                        DataContext = new FlasherOperationModel(_model),
                        Owner = this
                    };
                    if (eraseDlg.ShowDialog() != true) return;
                }

                var writeDlg = new WriteFlashWindow {
                    DataContext = new FlasherOperationModel(Model),
                    Owner = this
                };
                if (writeDlg.ShowDialog() == true && _model.Config.AutoVerify) {
                    VerifyFlashCommand(this, null);
                };
            }
        }

        private void WriteEepromCommand(object sender, ExecutedRoutedEventArgs e) {
            var msgResult = MessageBox.Show("Are you sure you want start writing eeprom. Device may become unoperable", "Eeprom confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (msgResult == MessageBoxResult.Yes) {
                var dlg = new WriteEepromWindow {
                    DataContext = new FlasherOperationModel(Model),
                    Owner = this
                };
                if (dlg.ShowDialog() == true && _model.Config.AutoVerify) {
                    VerifyEepromCommand(this, null);
                };
            }
        }

        private void WriteLockBitsCommand(object sender, ExecutedRoutedEventArgs e) {
            var msgResult = MessageBox.Show("Are you sure you want start writing lock bits. Data may become unreadable", "Lock bits confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (msgResult == MessageBoxResult.Yes) {
                var dlg = new WriteLocksWindow {
                    DataContext = new FlasherOperationModel(Model),
                    Owner = this
                };
                if (dlg.ShowDialog() == true && _model.Config.AutoVerify) {
                    VerifyLockBitsCommand(this, null);
                };
            }
        }

        private void WriteFuseBitsCommand(object sender, ExecutedRoutedEventArgs e) {
            var msgResult = MessageBox.Show("Are you sure you want start writing fuse bits. Device may become unoperable", "Fuse bits confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (msgResult == MessageBoxResult.Yes) {
                var dlg = new WriteFusesWindow {
                    DataContext = new FlasherOperationModel(Model),
                    Owner = this
                };
                if (dlg.ShowDialog() == true && _model.Config.AutoVerify) {
                    VerifyFuseBitsCommand(this, null);
                };
            }
        }

        #endregion

        #region Verify

        private void VerifyDeviceCommand(object sender, ExecutedRoutedEventArgs e) {
            var dlg = new VerifyDeviceWindow {
                DataContext = new FlasherOperationModel(_model),
                Owner = this
            };
            dlg.ShowDialog();
        }

        private void VerifyFlashCommand(object sender, ExecutedRoutedEventArgs e) {
            var dlg = new VerifyFlashWindow {
                DataContext = new FlasherOperationModel(_model),
                Owner = this
            };
            dlg.ShowDialog();
        }

        private void VerifyEepromCommand(object sender, ExecutedRoutedEventArgs e) {
            var dlg = new VerifyEepromWindow {
                DataContext = new FlasherOperationModel(_model),
                Owner = this
            };
            dlg.ShowDialog();
        }

        private void VerifyLockBitsCommand(object sender, ExecutedRoutedEventArgs e) {
            var dlg = new VerifyLockBitsWindow {
                DataContext = new FlasherOperationModel(_model),
                Owner = this
            };
            dlg.ShowDialog();
        }

        private void VerifyFuseBitsCommand(object sender, ExecutedRoutedEventArgs e) {
            var dlg = new VerifyFuseBitsWindow {
                DataContext = new FlasherOperationModel(_model),
                Owner = this
            };
            dlg.ShowDialog();
        }

        #endregion

        private void SettingsCommand(object sender, ExecutedRoutedEventArgs e) {
            var settings = FlasherConfig.Read();
            var oldDevice = settings.Device.Name;
            var dlg = new SettingsWindow {
                DataContext = settings,
                Owner = this
            };
            if (dlg.ShowDialog() ?? false) {
                settings.Save();
                if (settings.Device.Name != oldDevice) {
                    Model.ClearAll();
                }
                Model.ReloadConfig();
            }
        }

        private void LockBitsCommand(object sender, ExecutedRoutedEventArgs e) {
            var dlg = new LockBitsWindow {
                DataContext = _model,
                Owner = this
            };
            dlg.ShowDialog();
        }

        private void FuseBitsCommand(object sender, ExecutedRoutedEventArgs e) {
            var dlg = new FuseBitsWindow {
                DataContext = _model,
                Owner = this
            };
            dlg.ShowDialog();
        }

        private void EraseDeviceCommand(object sender, ExecutedRoutedEventArgs e) {
            var msgResult = MessageBox.Show("Are you sure you want start erasing the device. All previous data will be lost", "Erase confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (msgResult == MessageBoxResult.Yes) {
                var dlg = new EraseDeviceWindow {
                    DataContext = new FlasherOperationModel(_model),
                    Owner = this
                };
                dlg.ShowDialog();
            }
        }

        private void ResetDevice(object sender, ExecutedRoutedEventArgs e) {
            var dlg = new ResetDeviceWindow {
                DataContext = new FlasherOperationModel(_model),
                Owner = this
            };
            dlg.ShowDialog();
        }

        private void SerialMonitor(object sender, ExecutedRoutedEventArgs e) {
            var dlg = new SerialMonitorWindow {
                DataContext = _model,
                Owner = this
            };
            dlg.ShowDialog();
        }

        private void AboutCommand(object sender, ExecutedRoutedEventArgs e) {
            var dlg = new AboutWindow {
                DataContext = _model,
                Owner = this
            };
            dlg.ShowDialog();
        }
    }
}
