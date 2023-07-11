using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Win32;
using refShop_DEV.Models.Login;
using refShop_DEV.Models.MyDbContext;
using refShop_DEV.Models.Restaurant;

namespace refShop_DEV.Services
{
    public class RegistroActividadUpdater
    {
        private readonly MyDbContext _context;
        private readonly User _user;


        public RegistroActividadUpdater(MyDbContext context,User user)
        {
            _context = context;
            _user = user;
            
        }

        public void Start()
        {
            // Configura el temporizador para que se ejecute cada minuto
            var timer = new Timer(UpdateRegistroActividad, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        }

        private void UpdateRegistroActividad(object state)
        {
            // Obtiene la fecha y hora actual
            var now = DateTime.Now.TimeOfDay;
            var nowDate = DateTime.Now;

            // Verifica si el usuario existe
            if (_user == null)
            {
                // Maneja el caso en el que el usuario no existe
                return;
            }

            // Verifica si el usuario tiene un turno asignado
            if (_user.Turno != null)
            {

                // Obtener el turno asignado al usuario en curso
                Turno turnoEnCurso = _user.Turno;



                // Verifica si el turno está en curso
                if (turnoEnCurso.HoraInicio <= now && turnoEnCurso.HoraFin >= now)
                {
                    // Verifica si ya existe un registro de actividad para este turno y empleado
                    var registro = _context.RegistrosActividad
                        .FirstOrDefault(r => r.ID_Empleado == _user.Id && r.IDTurno == turnoEnCurso.IDTurno);

                    if (registro == null)
                    {
                        // No hay registro de actividad, crea uno nuevo
                        registro = new RegistroActividad
                        {
                            ID_Empleado = _user.Id,
                            IDTurno = turnoEnCurso.IDTurno,
                            Fecha_HoraInicio = nowDate,
                            Conectado = true,
                            Horas_Extras = CalcularHorasExtras(now, turnoEnCurso), // Calcula las horas extras
                            Horas_Faltantes = CalcularHorasFaltantes(now, turnoEnCurso), // Calcula las horas faltantes
                            Demora_Inicio = CalcularDemoraInicio(now, turnoEnCurso), // Calcula la demora en el inicio
                            Justificado = VerificarSiEstaJustificado(), // Verifica si está justificado
                            Comentario = ObtenerComentario() // Obtiene el comentario correspondiente
                        };

                        _context.RegistrosActividad.Add(registro);
                    }
                    else
                    {
                        // Ya hay un registro de actividad, actualízalo si es necesario
                        if (registro.Fecha_HoraFin == null)
                        {
                            registro.Fecha_HoraFin = nowDate;

                            // Calcula las horas extras si el usuario se desconectó después de la horaFin del turno
                            if (now > turnoEnCurso.HoraFin)
                            {
                                TimeSpan horasExtras = now - turnoEnCurso.HoraFin;
                                registro.Horas_Extras = Convert.ToDecimal(horasExtras.TotalHours);
                            }

                            // Resto de las actualizaciones necesarias
                        }
                    }


                    // Guarda los cambios en la base de datos
                }
                else
                {
                    // El turno no está en curso
                    // Realiza las operaciones correspondientes para el caso de turno no válido

                    // Resto de las actualizaciones necesarias si es necesario

                    if (now > turnoEnCurso.HoraFin)
                    {
                        TimeSpan horasExtras = now - turnoEnCurso.HoraFin;
                        var RegistroActividad = new RegistroActividad()
                        {
                            ID_Empleado = _user.Id,
                            IDTurno = _user.Turno.IDTurno,
                            Fecha_HoraInicio = DateTime.Now,
                            Fecha_HoraFin = null,
                            Conectado = true,
                            Horas_Extras = CalcularHorasExtras(now, turnoEnCurso), // Calcula las horas extras
                            Horas_Faltantes = CalcularHorasFaltantes(now, turnoEnCurso), // Calcula las horas faltantes
                            Demora_Inicio = CalcularDemoraInicio(now, turnoEnCurso), // Calcula la demora en el inicio
                            Justificado = VerificarSiEstaJustificado(), // Verifica si está justificado
                            Comentario = ObtenerComentario() // Obtiene el comentario correspondiente

                        };

                       
                        _context.RegistrosActividad.Add(RegistroActividad);

                    }
                }

            }

        }

        private decimal CalcularHorasExtras(TimeSpan now, Turno turno)
        {
            // Verifica si el usuario se desconectó después de la horaFin del turno
            if (now > turno.HoraFin)
            {
                TimeSpan horasExtras = now - turno.HoraFin;
                return Convert.ToDecimal(horasExtras.TotalHours);
            }

            return 0;
        }

        private decimal CalcularHorasFaltantes(TimeSpan now, Turno turno)
        {
            // Verifica si el usuario está dentro del horario del turno
            if (turno.HoraInicio <= now && turno.HoraFin >= now)
            {
                return 0;
            }

            // Calcula las horas faltantes como la diferencia entre la hora actual y la horaFin del turno
            TimeSpan horasFaltantes = turno.HoraFin - now;
            return Convert.ToDecimal(horasFaltantes.TotalHours);
        }

        private TimeSpan CalcularDemoraInicio(TimeSpan now, Turno turno)
        {
            // Verifica si el usuario se conectó después de la horaInicio del turno
            if (now > turno.HoraInicio)
            {
                return now - turno.HoraInicio;
            }

            return TimeSpan.Zero;
        }

        private bool VerificarSiEstaJustificado()
        {
            // Implementa aquí tu lógica específica para determinar si la hora está justificada
            // Por ejemplo, puedes consultar una tabla de justificaciones o implementar reglas específicas
            return false; // Valor de ejemplo, reemplaza con la lógica adecuada
        }

        private string ObtenerComentario()
        {
            // Implementa aquí tu lógica específica para obtener el comentario correspondiente
            // Por ejemplo, puedes permitir que el usuario ingrese un comentario o usar un campo predefinido
            return string.Empty; // Valor de ejemplo, reemplaza con la lógica adecuada
        }

        public bool TerminarTurno(User user, DbContext _context)
        {

            return true;

        }


    }

}
