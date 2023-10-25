﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

/**
 * El método del drop-área ha sido confeccionado con la ayuda del modelo de lenguaje GPT 3.5.
 * Los comentarios correspondientes a esa parte del código son confeccionados por mí para demostrar el entendimiento
 * del fragmento de código y su integración, así como para facilitar su comprensión para reutilización de a futuro.
 */

namespace CompresorArchivosWPF
{
    public partial class MainWindow : Window
    {
        // Lista de imágenes recientes.
        private List<string> imagePaths = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            AllowDrop = true;
            Drop += ImageDrop; // Se llama al método al soltar un archivo en la ventana. Se puede hacer así o poner el enlace al método del evento en el componente directamente en el XAML. Lo destaco porque me he fijado en ello a raíz de este proyecto.
        }

        // Se añade la imagen dropeada en el drop-área a la lista de recientes y se muestra en el marco.
        private void ImageDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (string file in files)
                {
                    if (IsImageFile(file))
                    {
                        imagePaths.Add(file); // Se añade a la lista de rutas de recientes.
                        AddRecentImageMenuItem(file); // Añade un nuevo componente a la lista de recientes que contendrá la ruta de la nueva imagen reciente.
                        CompressFile(file); // La imagen se muestra en el visualizador.
                    }
                }
            }
        }

        // Muestra la imagen recibida por parámetros en forma de path en el visualizador.
        private void CompressFile(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {

            }
        }

        // Se comprueba que el archivo dropeado sea un archivo de imagen.
        private bool IsImageFile(string filePath)
        {
            string extension = System.IO.Path.GetExtension(filePath).ToLower();
            return extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif" || extension == ".bmp" || extension == ".webp";
        }

        // Método que abre la imagen reciente al clickar en el componente con la ruta de la imagen añadida anteriormente.
        private void OpenImage_Click(object sender, RoutedEventArgs e)
        {
            string imagePath = ((MenuItem)sender).Tag.ToString();
            DisplayImage(imagePath);
        }

        // Método que confecciona dinámicamente y añade componentes de path recientes al menú de recientes.
        private void AddRecentImageMenuItem(string filePath)
        {
            MenuItem recentFileMenuItem = new MenuItem();
            recentFileMenuItem.Header = System.IO.Path.GetFileName(filePath);
            recentFileMenuItem.Tag = filePath;
            recentFileMenuItem.Click += OpenImage_Click; // Se enlaza el evento al componente de forma dinámica por código.

            // Agregar el elemento al submenú "Recientes". Si sobrepasa los 20 elementos, borra el más antiguo.
            if (menuRecientes.Items.Count >= 20)
            {
                menuRecientes.Items.RemoveAt(19);
            }
            menuRecientes.Items.Insert(0, recentFileMenuItem);
        }
    }
}
