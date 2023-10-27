using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace CompresorArchivosWPF
{
    public partial class MainWindow : Window
    {
        string[] files;

        public MainWindow()
        {
            InitializeComponent();
            AllowDrop = true;
            Drop += FileDrop;
        }
        private void FileDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                files = (string[])e.Data.GetData(DataFormats.FileDrop);

                string archivoZip = "out\\" + GenerarNombreArchivoUnico();
                ComprimirArchivos(files, archivoZip);

            }
        }
        private void NuevoArchivoComprimido_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                Multiselect = true,
                Title = "Selecciona archivos para comprimir",
                Filter = "Archivos (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string archivoZip = "out\\" + GenerarNombreArchivoUnico(); // Ruta y nombre del archivo ZIP
                ComprimirArchivos(openFileDialog.FileNames, archivoZip);
            }
        }
        private static void ComprimirArchivos(string[] archivos, string archivoZip)
        {
            try
            {
                using (ZipArchive archivo = ZipFile.Open(archivoZip, ZipArchiveMode.Update))
                {
                    foreach (string archivoAComprimir in archivos)
                    {
                        string nombreArchivoEnZip = Path.GetFileName(archivoAComprimir);
                        archivo.CreateEntryFromFile(archivoAComprimir, nombreArchivoEnZip);
                    }
                }

                string recientesFile = "res\\recientes.txt";
                EscribirRecientesEnTxt(archivos, recientesFile);

                MessageBox.Show("Archivos comprimidos con éxito.");
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error al comprimir archivos: " + exception.Message);
            }
        }
        private static void EscribirRecientesEnTxt(string[] rutasArchivos, string recientesFile)
        {
            try
            {
                using (StreamWriter writer = new(recientesFile, true))
                {
                    foreach (string rutaArchivo in rutasArchivos)
                    {
                        writer.WriteLine(rutaArchivo);
                    }
                }
                MessageBox.Show("Rutas guardadas en el archivo de texto.");
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error al guardar las rutas en el archivo de texto: " + exception.Message);
            }
        }
        private void ListarArchivosComprimidos_Click(object sender, RoutedEventArgs e)
        {
            string recientesFile = "res\\recientes.txt";
            MessageBox.Show("Lista de archivos recientes generada.");

            if (File.Exists(recientesFile))
            {
                
                string[] recientesOut = File.ReadAllLines(recientesFile);
                listBox.Items.Clear();
                foreach (string line in recientesOut)
                {
                    ListBoxItem listBoxItem = new()
                    {
                        Content = line,
                        Tag = line
                    };
                    listBox.Items.Add(listBoxItem);
                }
            }
            else
            {
                MessageBox.Show("No se encontró el archivo de lista de archivos comprimidos.");
            }
        }

        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Deseas cerrar la aplicación?", "Salir", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Close();
            }
        }
        private static string GenerarNombreArchivoUnico()
        {
            // Genera un nombre de archivo basado en la fecha y hora actual
            string fechaHoraActual = DateTime.Now.ToString("yyyyMMddHHmmss");
            return "archivo_" + fechaHoraActual + ".zip";
        }
    }
}
