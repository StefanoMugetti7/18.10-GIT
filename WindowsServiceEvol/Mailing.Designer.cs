namespace WindowsServiceEvol
{
    partial class Mailing
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.timerMailing = new System.Timers.Timer();
            ((System.ComponentModel.ISupportInitialize)(this.timerMailing)).BeginInit();
            // 
            // timerMailing
            // 
            this.timerMailing.Enabled = true;
            this.timerMailing.Interval = 5000D;
            this.timerMailing.Elapsed += new System.Timers.ElapsedEventHandler(this.timerMailing_Elapsed);
            // 
            // Mailing
            // 
            this.ServiceName = "MailingEvol";
            ((System.ComponentModel.ISupportInitialize)(this.timerMailing)).EndInit();

        }

        #endregion

        private System.Timers.Timer timerMailing;
    }
}
