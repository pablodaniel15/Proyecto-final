using Expresso.Model;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;


namespace Expresso.ViewModel
{
    public class RregistroProductoViewModel : INotifyPropertyChanged
    {
        private readonly HttpClient _httpClient;
        public Producto Producto { get; set; } = new Producto();
        public ICommand RegistrarProductoCommand { get; }
        public ICommand EliminarProductoCommand { get; }
        public ICommand EditarProductoCommand { get; }
        public ICommand ObtenerProductoCommand { get; }

        public event PropertyChangedEventHandler? PropertyChanged; 

        public RregistroProductoViewModel()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://192.168.100.101:7061")
            };

            RegistrarProductoCommand = new Command(async () => await RegistrarProducto());
            EliminarProductoCommand = new Command<int>(async (id) => await EliminarProducto(id));
            EditarProductoCommand = new Command(async () => await EditarProducto());
            ObtenerProductoCommand = new Command<int>(async (id) => await ObtenerProducto(id));
        }

        private async Task RegistrarProducto()
        {
            try
            {
                if (!string.IsNullOrEmpty(Producto.Nombre) && Producto.Precio > 0)
                {
                    var response = await _httpClient.PostAsJsonAsync("api/productos", Producto);

                    if (response.IsSuccessStatusCode)
                    {
                        Producto = new Producto();
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Producto)));
                    }
                    else
                    {
                        Console.WriteLine($"Error al registrar producto: {response.StatusCode}");
                    }
                }
                else
                {
                    Console.WriteLine("Por favor, asegúrate de que el nombre y el precio del producto son válidos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        private async Task EliminarProducto(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/productos/{id}");

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Producto eliminado exitosamente.");
                    // Aquí podrías actualizar la lista de productos si estás manejando una
                }
                else
                {
                    Console.WriteLine($"Error al eliminar producto: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        private async Task EditarProducto()
        {
            try
            {
                if (!string.IsNullOrEmpty(Producto.Nombre) && Producto.Precio > 0)
                {
                    var response = await _httpClient.PutAsJsonAsync($"api/productos/{Producto.Id}", Producto);

                    if (response.IsSuccessStatusCode)
                    {
                        Producto = new Producto();
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Producto)));
                    }
                    else
                    {
                        Console.WriteLine($"Error al editar producto: {response.StatusCode}");
                    }
                }
                else
                {
                    Console.WriteLine("Por favor, asegúrate de que el nombre y el precio del producto son válidos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        private async Task ObtenerProducto(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/productos/{id}");

                if (response.IsSuccessStatusCode)
                {
                    Producto = await response.Content.ReadFromJsonAsync<Producto>();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Producto)));
                }
                else
                {
                    Console.WriteLine($"Error al obtener producto: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.InnerException?.Message ?? ex.Message}");
            }
        }
    }
}
