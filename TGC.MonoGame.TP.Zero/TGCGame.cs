using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP.Zero;

/// <summary>
///     Clase principal del juego.
/// </summary>
public class TGCGame : Game
{
    public const string ContentFolder3D = "Models/";
    public const string ContentFolderEffects = "Effects/";
    public const string ContentFolderMusic = "Music/";
    public const string ContentFolderSounds = "Sounds/";
    public const string ContentFolderSpriteFonts = "SpriteFonts/";
    public const string ContentFolderTextures = "Textures/";

    private Model _carModel;
    private Matrix _carWorld;
    private CityScene _city;
    private FollowCamera _followCamera;

    // Variables de estado del auto
    
    // Traslacion
    private Vector3 _carPosition; // Guarda la posicion actual del auto
    private float _carSpeed; // Velocidad con la que se desplaza

    private const float Acceleration = 500f; // Aceleracion con la que cambia la velocidad
    private const float Friction = 0.98f; // Friccion que hace que el auto pierda velocidad
    
    // Rotacion
    private float _carRotation; // Guarda la rotacion actual del auto
    private float _carRotationSpeed; // Velocidad con la que gira
    
    // Salto
    private bool _isJumping; // Indica si el auto esta saltando
    private float _verticalSpeed; // Velocidad con la que salta

    private const float _gravity = 500f; // Aceleracion con la que cae

    private readonly GraphicsDeviceManager _graphics;

    /// <summary>
    ///     Constructor del juego.
    /// </summary>
    public TGCGame()
    {
        // Se encarga de la configuracion y administracion del Graphics Device.
        _graphics = new GraphicsDeviceManager(this);

        // Carpeta donde estan los recursos que vamos a usar.
        Content.RootDirectory = "Content";

        // Hace que el mouse sea visible.
        IsMouseVisible = true;
    }

    /// <summary>
    ///     Llamada una vez en la inicializacion de la aplicacion.
    ///     Escribir aca todo el codigo de inicializacion: Todo lo que debe estar precalculado para la aplicacion.
    /// </summary>
    protected override void Initialize()
    {
        // Enciendo Back-Face culling.
        // Configuro Blend State a Opaco.
        var rasterizerState = new RasterizerState();
        rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
        GraphicsDevice.RasterizerState = rasterizerState;
        GraphicsDevice.BlendState = BlendState.Opaque;

        // Configuro las dimensiones de la pantalla.
        _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100;
        _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;
        _graphics.ApplyChanges();

        // Creo una camara para seguir a nuestro auto.
        _followCamera = new FollowCamera(GraphicsDevice.Viewport.AspectRatio);

        // Configuro la matriz de mundo del auto.
        _carWorld = Matrix.Identity;

        // Inicializo las variables de estado del auto.
        _carPosition = Vector3.Zero;
        _carSpeed = 0f;

        _carRotation = 0f;
        _carRotationSpeed = 3f;

        _isJumping = false;
        _verticalSpeed = 0f;

        base.Initialize();
    }

    /// <summary>
    ///     Llamada una sola vez durante la inicializacion de la aplicacion, luego de Initialize, y una vez que fue configurado
    ///     GraphicsDevice.
    ///     Debe ser usada para cargar los recursos y otros elementos del contenido.
    /// </summary>
    protected override void LoadContent()
    {
        // Creo la escena de la ciudad.
        _city = new CityScene(Content, ContentFolder3D, ContentFolderEffects);

        // La carga de contenido debe ser realizada aca.
        _carModel = Content.Load<Model>(ContentFolder3D + "scene/car");

        base.LoadContent();
    }

    /// <summary>
    ///     Es llamada N veces por segundo. Generalmente 60 veces pero puede ser configurado.
    ///     La logica general debe ser escrita aca, junto al procesamiento de mouse/teclas.
    /// </summary>
    protected override void Update(GameTime gameTime)
    {
        // Variables de estado del auto locales
        
        Matrix rotationMatrix = Matrix.CreateRotationY(_carRotation);
        // Se toma en cuenta que la matriz de rotacion contiene la direccion a la que apunta el frente del auto
        Vector3 carForward = rotationMatrix.Forward;

        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds; 
        // Segundos transcurridos desde el fotograma anterior
        // Esto es para que el juego corra a la misma velocidad en cualquier computadora

        // Capturo el estado del teclado.
        var keyboardState = Keyboard.GetState();
        if (keyboardState.IsKeyDown(Keys.Escape))
        {
            // Salgo del juego.
            Exit();
        }

        // La logica debe ir aca.

        // Para la traslacion se modifica el valor de las coordenadas usando la velocidad
        // La velocidad se modificara en base a la aceleracion que se aplique con las teclas W y S

        // Acelerar con W
        if (keyboardState.IsKeyDown(Keys.W))
        {
            _carSpeed += Acceleration * deltaTime;
        }
        // Desacelerar con S
        if (keyboardState.IsKeyDown(Keys.S))
        {
            _carSpeed -= Acceleration * deltaTime;
        }

        // Aplico la friccion del suelo al auto
        _carSpeed *= Friction;

        // Aplico la velocidad al auto segun el cambio de aceleracion
        _carPosition += carForward * _carSpeed * deltaTime;

        // Para la rotacion se modifica el valor de la rotacion usando la velocidad de rotacion

        // Girar a la derecha con D
        if (keyboardState.IsKeyDown(Keys.D))
        {
            _carRotation -= _carRotationSpeed * deltaTime; 
            // Actualizo la rotacion del auto usando la velocidad de rotacion
            // Se resta porque la rotacion es en sentido anti-horario por la regla de la mano derecha
            // Si giramos a la derecha, el angulo de rotacion disminuye
        }
        // Girar a la izquierda con A
        if (keyboardState.IsKeyDown(Keys.A))
        {
            _carRotation += _carRotationSpeed * deltaTime; 
            // Actualizo la rotacion del auto usando la velocidad de rotacion
            // Se suma porque la rotacion es en sentido anti-horario por la regla de la mano derecha
            // Si giramos a la izquierda, el angulo de rotacion aumenta
        }

        // Para el salto se modifica el valor de la velocidad vertical usando la gravedad

        // Salto con Espacio
        if (keyboardState.IsKeyDown(Keys.Space) && !_isJumping)
        {
            _isJumping = true;
            _verticalSpeed = 200f;

        }
        if (_isJumping)
        {
            _carPosition.Y += _verticalSpeed * deltaTime; // Otorga el impulso hacia arriba con la velocidad vertical
            _verticalSpeed -= _gravity * deltaTime; // Disminuye la velocidad vertical debido a la gravedad
            if (_carPosition.Y <= 0) // Si el auto queda "debajo" del suelo
            {
                _carPosition.Y = 0; // Lo posiciona en el suelo
                _isJumping = false; // Deja de saltar
            }
        }

        // Actualizo la matriz de mundo del auto con las modificaciones hechas por las teclas
        _carWorld = rotationMatrix * Matrix.CreateTranslation(_carPosition);

        // Actualizo la camara, enviandole la matriz de mundo del auto.
        _followCamera.Update(gameTime, _carWorld);

        base.Update(gameTime);
    }
    
    /// <summary>
    ///     Llamada para cada frame.
    ///     La logica de dibujo debe ir aca.
    /// </summary>
    protected override void Draw(GameTime gameTime)
    {
        // Limpio la pantalla.
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // Dibujo la ciudad.
        _city.Draw(gameTime, _followCamera.View, _followCamera.Projection);

        // El dibujo del auto debe ir aca.
        _carModel.Draw(_carWorld, _followCamera.View, _followCamera.Projection);

        base.Draw(gameTime);
    }

    /// <summary>
    ///     Libero los recursos cargados.
    /// </summary>
    protected override void UnloadContent()
    {
        // Libero los recursos cargados dessde Content Manager.
        Content.Unload();

        base.UnloadContent();
    }
}