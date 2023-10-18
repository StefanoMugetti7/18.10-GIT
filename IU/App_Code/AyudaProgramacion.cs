using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using AjaxControlToolkit;
using Comunes.Entidades;
using Generales.Entidades;
using Afiliados.Entidades;
using System.Web.UI.HtmlControls;
using Tesorerias.Entidades;
using CuentasPagar.Entidades;
using Cobros.Entidades;
using Prestamos.Entidades;

namespace IU
{
    public class AyudaProgramacion
    {
        //public static void CompletarDropConEnum(DropDownList pDrop, string[] pNames)
        //{
        //    foreach (string sTemp in pNames)
        //    {
        //        string sNombre = "";
        //        bool fLower = char.IsUpper(sTemp[0]);
        //        foreach (char cTemp in sTemp)
        //        {
        //            if (char.IsLower(cTemp))
        //            {
        //                sNombre = sNombre + cTemp;
        //                fLower = true;
        //            }
        //            else
        //            {
        //                if (fLower)
        //                {
        //                    sNombre = sNombre + " " + cTemp;
        //                }
        //                else
        //                {
        //                    sNombre = sNombre + cTemp;
        //                }
        //                fLower = false;
        //            }
        //        }
        //        pDrop.Items.Add ( new ListItem (sNombre,sTemp));
        //    }
        //}

        public static void CompletarDropConEnum(DropDownList pDrop, Type pEnum)
        {
            pDrop.Items.Clear();

            Type tipoEnum = pEnum;
            Array valores = Enum.GetValues(tipoEnum);
            string sTemp = string.Empty;
            string sNombre;
            int val = 0;

            for (int i = 0; i < valores.Length; i++)
            {
                sNombre = string.Empty;
                sTemp = Enum.GetName(tipoEnum, valores.GetValue(i));
                bool fLower = char.IsUpper(sTemp[0]);
                foreach (char cTemp in sTemp)
                {
                    if (char.IsLower(cTemp))
                    {
                        sNombre = sNombre + cTemp;
                        fLower = true;
                    }
                    else
                    {
                        if (fLower)
                        {
                            sNombre = sNombre + " " + cTemp;
                        }
                        else
                        {
                            sNombre = sNombre + cTemp;
                        }
                        fLower = false;
                    }
                }
                val = (int)valores.GetValue(i);
                pDrop.Items.Add(new ListItem(sNombre, val.ToString()));
            }
        }

        //public static void CompletarDropConEnumEstadosBD(DropDownList pDrop, Type pEnum)
        //{
        //    pDrop.Items.Clear();

        //    Type tipoEnum = pEnum;
        //    Array valores = Enum.GetValues(tipoEnum);
        //    ListItem item;
        //    int id;
        //    List<TGEEstados> estados = TGEGeneralesF.TGEEstadosObtenerLista();
        //    TGEEstados estado;

        //    for (int i = 0; i < valores.Length; i++)
        //    {
        //        id = Convert.ToInt32(valores.GetValue(i));
        //        item = new ListItem();
        //        estado = estados.Find(x=>x.IdEstado==id);
        //        item.Value = estado.IdEstado.ToString();
        //        item.Text=estado.Descripcion;
        //        pDrop.Items.Add(item);
        //    }
        //}

        //public static void CompletarDropConEnum(AjaxControlToolkit.ComboBox pDrop, string[] pNames)
        //{
        //    foreach (string sTemp in pNames)
        //    {
        //        string sNombre = "";
        //        bool fLower = char.IsUpper(sTemp[0]);
        //        foreach (char cTemp in sTemp)
        //        {
        //            if (char.IsLower(cTemp))
        //            {
        //                sNombre = sNombre + cTemp;
        //                fLower = true;
        //            }
        //            else
        //            {
        //                if (fLower)
        //                {
        //                    sNombre = sNombre + " " + cTemp;
        //                }
        //                else
        //                {
        //                    sNombre = sNombre + cTemp;
        //                }
        //                fLower = false;
        //            }
        //        }
        //        pDrop.Items.Add( new ListItem(sNombre, sTemp));
        //    }
        //}

        /// <summary>
        /// Establece el estado de los controles y los controles incluidos en la 
        /// coleccion de controles
        /// </summary>
        /// <param name="pControl"></param>
        /// <param name="pEstado"></param>
        public static void HabilitarControles(Control pControl, bool pEstado, PaginaSegura pPaginaSegura)
        {
            bool estado;
            SegControlesPaginas ctrlSeguro;

            Menues menu = pPaginaSegura.UsuarioActivo.Menues.Find(delegate(Menues me)
            { return pPaginaSegura.paginaActual.URL.Equals(me.URL); });

            if (menu == null)
                return;

            foreach (Control ctrl in pControl.Controls)
            {
                ctrlSeguro = menu.ControlesPaginas.Find(delegate(SegControlesPaginas ctrlSeg)
                { return ctrlSeg.ControlesPaginas == ctrl.ID; });

                if (ctrlSeguro != null && !ctrlSeguro.TienePermiso)
                    estado = false;
                else
                    estado = pEstado;

                if (ctrl is Button && ctrl.ID != "btnCancelar")
                    ((Button)ctrl).Enabled = estado;
                //if (ctrl is Image)
                //    ((Image)ctrl).Visible = estado;
                if (ctrl is TextBox)
                {
                    if (!(((TextBox)ctrl).Enabled == true))
                        ((TextBox)ctrl).Enabled = estado;
                    //((TextBox)ctrl).Enabled = estado;
                }
                if (ctrl is CheckBox && ctrl.ID != "chkMostrarAsiento")
                    ((CheckBox)ctrl).Enabled = estado;
                if (ctrl is DropDownList)
                {
                    //if (((DropDownList)ctrl).Enabled != false)
                    ((DropDownList)ctrl).Enabled = estado;
                }
                if (ctrl is CheckBoxList)
                    ((CheckBoxList)ctrl).Enabled = estado;
                if (ctrl is TreeView)
                    ((TreeView)ctrl).Enabled = estado;
                //if (ctrl is Panel)
                //    ((Panel)ctrl).Enabled = estado;
                if (ctrl is ComboBox)
                    ((ComboBox)ctrl).Enabled = estado;
                if (ctrl is CalendarExtender)
                    ((CalendarExtender)ctrl).Enabled = estado;

                if (ctrl.Controls != null && ctrl.Controls.Count > 0)
                    HabilitarControles(ctrl, pEstado, pPaginaSegura);
            }
        }

        /// <summary>
        /// Deshabilita y oculta controles que tienen seguridad
        /// </summary>
        /// <param name="pControl"></param>
        /// <param name="pEstado"></param>
        public static void DeshabilitarControlesSeguros(Control pControl, Menues pMenu)
        {
            List<Control> listaCtrl = new List<Control>();
            foreach (SegControlesPaginas ctrlPag in pMenu.ControlesPaginas)
            {
                if (!ctrlPag.TienePermiso)
                {
                    listaCtrl = FindControlRecursive(pControl, ctrlPag.ControlesPaginas);

                    foreach (Control ctrlWeb in listaCtrl)
                    {
                        if (ctrlWeb == null)
                            break;

                        if (ctrlWeb is Button)
                            ((Button)ctrlWeb).Style.Add("visibility", "hidden");
                        //((Button)ctrlWeb).Visible = false;
                        if (ctrlWeb is ImageButton)
                            ((ImageButton)ctrlWeb).Style.Add("visibility", "hidden");
                        //((ImageButton)ctrlWeb).Visible = false;
                        if (ctrlWeb is Image)
                            ((Image)ctrlWeb).Visible = false;
                        if (ctrlWeb is TextBox)
                            ((TextBox)ctrlWeb).Enabled = false;
                        if (ctrlWeb is CheckBox)
                            ((CheckBox)ctrlWeb).Enabled = false;
                        if (ctrlWeb is DropDownList)
                            ((DropDownList)ctrlWeb).Enabled = false;
                        if (ctrlWeb is CheckBoxList)
                            ((CheckBoxList)ctrlWeb).Enabled = false;
                        if (ctrlWeb is TreeView)
                            ((TreeView)ctrlWeb).Enabled = false;
                        if (ctrlWeb is Panel)
                            ((Panel)ctrlWeb).Enabled = false;
                        if (ctrlWeb is CalendarExtender)
                            ((CalendarExtender)ctrlWeb).Enabled = false;
                    }
                }
            }
            listaCtrl = new List<Control>();
            foreach (MenuesControlesOcultar ctrlPag in pMenu.ControlesOcultar.Where(x => !x.Control.Contains("{")))
            {
                listaCtrl = FindControlRecursive(pControl, ctrlPag.Control);
                foreach (Control ctrlWeb in listaCtrl)
                {
                    if (ctrlWeb == null)
                        break;
                    ctrlWeb.Visible = false;
                }
            }
            string idGrilla;
            GridView gv;
            string[] columnas= { };
            int start, end;
            foreach (MenuesControlesOcultar ctrlPag in pMenu.ControlesOcultar.Where(x => x.Control.Contains("{")))
            {
                idGrilla = ctrlPag.Control.Substring(0, ctrlPag.Control.IndexOf("{"));
                listaCtrl = FindControlRecursive(pControl, idGrilla);
                foreach (Control ctrlWeb in listaCtrl)
                {
                    if (ctrlWeb == null)
                        break;
                    if (ctrlWeb is GridView)
                    {
                        gv = (GridView)ctrlWeb;
                        start = ctrlPag.Control.IndexOf("{") + 1;
                        end = ctrlPag.Control.IndexOf("}");
                        columnas = ctrlPag.Control.Substring(start, end - start).Split(',');
                        foreach (DataControlField col in gv.Columns)
                        {
                            if (columnas.ToList().Exists(x => x.Trim().ToLower() == col.HeaderText.Trim().ToLower()))
                                col.Visible = false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Devuelve un control buscado por Id de forma recursiva
        /// </summary>
        /// <param name="rootControl"></param>
        /// <param name="controlID"></param>
        /// <returns></returns>
        private static List<Control> FindControlRecursive(Control rootControl, string controlID)
        {
            List<Control> lista = new List<Control>();
            if (rootControl.ID == controlID)
                lista.Add(rootControl);

            foreach (Control controlToSearch in rootControl.Controls)
            {
                lista.AddRange(FindControlRecursive(controlToSearch, controlID));
            }
            return lista;
        }

        public static List<string> FindControlRecursiveAttributes(Control rootControl, string attributeName)
        {
            List<string> lista = new List<string>();
            if (rootControl is HtmlGenericControl) {
                if (!string.IsNullOrEmpty(((HtmlGenericControl)rootControl).Attributes[attributeName]))
                {
                        lista.Add(((HtmlGenericControl)rootControl).Attributes[attributeName]);
                }
            }

            foreach (Control controlToSearch in rootControl.Controls)
            {
                lista.AddRange(FindControlRecursiveAttributes(controlToSearch, attributeName));
            }
            return lista;
        }

        private static Control FindControlRecursiveFirst(Control rootControl, string controlID)
        {
            Control ctr = new Control();
            if (rootControl.ID == controlID)
                ctr = rootControl;

            foreach (Control controlToSearch in rootControl.Controls)
            {
                ctr = FindControlRecursiveFirst(controlToSearch, controlID);
                if (ctr.ID != string.Empty)
                    break;
            }
            return ctr;
        }

        public static object MagicallyCreateInstance(string className)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var type = assembly.GetTypes()
                .First(t => t.Name == className);

            return Activator.CreateInstance(type);
        }

        public static bool ValidarControlPagina(Control rootControl, string controlID)
        {
            bool valida = false;
            if (rootControl.ID == controlID)
                return true;

            foreach (Control controlToSearch in rootControl.Controls)
            {
                valida = ValidarControlPagina(controlToSearch, controlID);
                if (valida)
                    break;
            }
            return valida;
        }

        private static T FindControlGridView<T>(GridView gv, string controlID) where T : Control
        {
            foreach (GridViewRow row in gv.Rows)
            {
                T control = (T)row.FindControl(controlID);
                if (control != null)
                    return control;
            }
            return null;
        }

        /// <summary>
        /// Limpia los datos cargados o seleccionados en los controles.
        /// </summary>
        /// <param name="pControl"></param>
        /// <param name="pRecursivo"></param>
        public static void LimpiarControles(Control pControl, bool pRecursivo)
        {
            foreach (Control ctrl in pControl.Controls)
            {
                if (ctrl is TextBox)
                    ((TextBox)ctrl).Text = string.Empty;
                if (ctrl is CheckBox)
                    ((CheckBox)ctrl).Checked = false;
                if (ctrl is CheckBoxList)
                    ((CheckBoxList)ctrl).ClearSelection();
                if (ctrl is ComboBox)
                    ((ComboBox)ctrl).SelectedValue = string.Empty;
                if (ctrl is DropDownList)
                {
                    ((DropDownList)ctrl).Items.Clear();
                    ((DropDownList)ctrl).SelectedIndex = -1;
                    ((DropDownList)ctrl).SelectedValue = null;
                    ((DropDownList)ctrl).ClearSelection();
                }
                if (ctrl is GridView)
                {
                    ((GridView)ctrl).DataSource = null;
                    ((GridView)ctrl).DataBind();
                }

                if (pRecursivo && ctrl.Controls != null && ctrl.Controls.Count > 0)
                    LimpiarControles(ctrl, pRecursivo);
            }
        }

        /// <summary>
        /// Limpia los datos cargados en los TextBox.
        /// </summary>
        /// <param name="pControl"></param>
        /// <param name="pRecursivo"></param>
        public static void LimpiarControlesTextBox(Control pControl, bool pRecursivo)
        {
            foreach (Control ctrl in pControl.Controls)
            {
                if (ctrl is TextBox)
                    ((TextBox)ctrl).Text = string.Empty;

                if (pRecursivo && ctrl.Controls != null && ctrl.Controls.Count > 0)
                    LimpiarControles(ctrl, pRecursivo);
            }
        }

        /// <summary>
        /// Agrega un item y lo selecciona de forma predeterminada
        /// </summary>
        /// <param name="pDdl"></param>
        /// <param name="pMensaje"></param>
        public static void AgregarItemSeleccione(DropDownList pDdl, string pMensaje)
        {
            pDdl.Items.Add(new ListItem(pMensaje, ""));
            //pDdl.Items.Insert(0, new ListItem(pMensaje, ""));
            pDdl.SelectedValue = "";
        }

        /// <summary>
        /// Agrega un item y lo selecciona de forma predeterminada
        /// </summary>
        /// <param name="pCbx"></param>
        /// <param name="pMensaje"></param>
        public static void AgregarItemSeleccione(ComboBox pCbx, string pMensaje)
        {
            pCbx.Items.Add(new ListItem(pMensaje, ""));
            //pCbx.Items.Insert(0, new ListItem(pMensaje, ""));
            pCbx.SelectedValue = "";
        }

        /// <summary>
        /// Agrega un item en la posicion 0 y lo selecciona de forma predeterminada
        /// </summary>
        /// <param name="pDdl"></param>
        /// <param name="pMensaje"></param>
        public static void InsertarItemSeleccione(DropDownList pDdl, string pMensaje)
        {
            pDdl.Items.Insert(0, new ListItem(pMensaje, ""));
            pDdl.SelectedValue = "";
        }

        /// <summary>
        /// Agrega un item en la posicion 0 y lo selecciona de forma predeterminada
        /// </summary>
        /// <param name="pCbx"></param>
        /// <param name="pMensaje"></param>
        public static void InsertarItemSeleccione(ComboBox pCbx, string pMensaje)
        {
            pCbx.Items.Insert(0, new ListItem(pMensaje, ""));
            pCbx.SelectedValue = "";
        }

        /// <summary>
        /// Establece los valores de las propiedades de una Entidad en los controles de la interfaz.
        /// Usuario.Nombre -> txtNombre.text
        /// </summary>
        /// <param name="pObjeto"></param>
        /// <param name="pControl"></param>
        public static void MapearEntidadControles(Object pObjeto, Control pControl)
        {
            Type t2 = pObjeto.GetType();
            PropertyInfo[] prop2 = t2.GetProperties();

            foreach (Control ctrl in pControl.Controls)
            {
                if (ctrl is TextBox || ctrl is CheckBox || ctrl is DropDownList || ctrl is ComboBox)
                {
                    foreach (PropertyInfo prop in prop2)
                    {
                        if (MapearEntidadControles(string.Empty, pObjeto, ctrl, prop))
                            break;
                    }
                }
                if (ctrl.Controls != null && ctrl.Controls.Count > 0)
                    MapearEntidadControles(pObjeto, ctrl);
            }
        }

        private static bool MapearEntidadControles(string propPadre, Object pObjeto, Control ctrl, PropertyInfo prop)
        {
            if (ctrl.ID == null) return false;
            if (prop.GetValue(pObjeto, null) == null) return false;
            //if (ctrl.ID.Length > 3 && ctrl.ID.Substring(3, ctrl.ID.Length - 3) == prop.Name)
            if (ctrl.ID.Length > 3 && ctrl.ID.Substring(3, ctrl.ID.Length - 3) == string.Concat(propPadre, prop.Name))
            {
                if (ctrl is TextBox)
                {
                    if (prop.PropertyType == typeof(DateTime))
                    {
                        ((TextBox)ctrl).Text = ((DateTime)(prop.GetValue(pObjeto, null))).ToShortDateString();
                    }
                    else
                    {
                        ((TextBox)ctrl).Text = prop.GetValue(pObjeto, null).ToString();
                    }
                    return true;
                }
                else if (ctrl is CheckBox)
                {
                    if (ctrl.ID == "chkBajaLogica")
                    {
                        ((CheckBox)ctrl).Checked = !(bool)prop.GetValue(pObjeto, null);
                        return true;
                    }
                    else
                    {
                        ((CheckBox)ctrl).Checked = (bool)prop.GetValue(pObjeto, null);
                        return true;
                    }
                }
                else if (ctrl is DropDownList || ctrl is ComboBox)
                {
                    if (((DropDownList)ctrl).Items.FindByValue(prop.GetValue(pObjeto, null).ToString()) != null)
                    {
                        ((DropDownList)ctrl).SelectedValue = prop.GetValue(pObjeto, null).ToString();
                        return true;
                    }
                }
            }
            if (ctrl is DropDownList || ctrl is ComboBox)
            {
                if (ctrl.ID.Length > 3 && ctrl.ID.Substring(3, ctrl.ID.Length - 3) == prop.Name.Substring(2, prop.Name.Length - 2))
                {
                    ((DropDownList)ctrl).SelectedValue = prop.GetValue(pObjeto, null).ToString();
                    return true;
                }
            }
            if (prop.PropertyType.Namespace.Contains("Entidades"))
            {
                object objetoInterno = prop.GetValue(pObjeto, null);
                Type tipoObjetoInterno = objetoInterno.GetType();
                PropertyInfo[] propiedadesInterno = tipoObjetoInterno.GetProperties();
                foreach (PropertyInfo propInterna in propiedadesInterno)
                {
                    if (MapearEntidadControles(prop.Name, objetoInterno, ctrl, propInterna))
                        break;
                }
            }
            return false;
        }

        /// <summary>
        /// Esatblece los valores de los controles en las propiedades de la Entidad
        /// txtNombre.text -> Usuario.Nombre
        /// </summary>
        /// <param name="pControl"></param>
        /// <param name="pObjeto"></param>
        public static void MapearControlesEntidad(Control pControl, Object pObjeto)
        {
            Type t2 = pObjeto.GetType();
            PropertyInfo[] prop2 = t2.GetProperties();

            foreach (Control ctrl in pControl.Controls)
            {
                if (ctrl is TextBox || ctrl is CheckBox || ctrl is DropDownList || ctrl is ComboBox)
                {
                    foreach (PropertyInfo prop in prop2)
                    {
                        if (MapearControlesEntidad(string.Empty, ctrl, pObjeto, prop))
                            break;
                    }
                }
                if (ctrl.Controls != null && ctrl.Controls.Count > 0)
                    MapearControlesEntidad(ctrl, pObjeto);
            }
        }

        private static bool MapearControlesEntidad(string propPadre, Control ctrl, Object pObjeto, PropertyInfo prop)
        {
            if (ctrl.ID == null) return false;
            object result;
            if (ctrl.ID.Length > 3 && ctrl.ID.Substring(3, ctrl.ID.Length - 3) == string.Concat(propPadre, prop.Name))
            {
                result = ObtenerValor(ctrl);
                if (result != null)
                {
                    if (result.ToString() == string.Empty)
                    {
                        if (prop.PropertyType == typeof(System.Int32) || prop.PropertyType == typeof(System.Decimal) || prop.PropertyType == typeof(System.Double))
                            result = 0;
                    }
                    if (result.ToString() == string.Empty && prop.PropertyType == typeof(System.DateTime))
                    {
                        // Si es un campo de fecha y la propiedad no tiene valor
                        return false;
                    }
                    if (prop.PropertyType.IsEnum)
                    {
                        //Type tipo = prop.GetType();
                        //object val = Enum.Parse(tipo, result.ToString());
                        //prop.SetValue(pObjeto, Convert.ChangeType(val, prop.PropertyType), null);
                        return false;
                    }
                    if (prop.PropertyType == typeof(System.Decimal))
                    {
                        result = result.ToString().Replace("$", "");
                        prop.SetValue(pObjeto, Convert.ChangeType(result, prop.PropertyType), null);
                        return true;
                    }
                    if (prop.PropertyType.ToString() == "System.Nullable`1[System.Int32]")
                    {
                        if (result != null)
                            prop.SetValue(pObjeto, Convert.ToInt32(result), null);

                        return true;
                    }
                    if (prop.PropertyType.ToString() == "System.Nullable`1[System.DateTime]")
                    {
                        if (result != null )
                            prop.SetValue(pObjeto, Convert.ToDateTime(result), null);

                        return true;
                    }
                    prop.SetValue(pObjeto, Convert.ChangeType(result, prop.PropertyType), null);
                    return true;
                }
            }
            if (ctrl is DropDownList || ctrl is ComboBox)
            {
                if (ctrl.ID.Length > 3 && ctrl.ID.Substring(3, ctrl.ID.Length - 3) == string.Concat(propPadre, prop.Name))
                //if (ctrl.ID.Length > 3 && ctrl.ID.Substring(3, ctrl.ID.Length - 3) == string.Concat(propPadre, prop.Name.Substring(2, prop.Name.Length - 2)))
                //if (ctrl.ID.Length > 3 && ctrl.ID.Substring(3, ctrl.ID.Length - 3) == prop.Name.Substring(2, prop.Name.Length - 2))
                {
                    result = ObtenerValor(ctrl);
                    if (result != null)
                    {
                        if (result.ToString() == string.Empty)
                        {
                            if (prop.PropertyType == typeof(System.Int32) || prop.PropertyType == typeof(System.Decimal) || prop.PropertyType == typeof(System.Double))
                                result = 0;
                        }
                        prop.SetValue(pObjeto, Convert.ChangeType(result, prop.PropertyType), null);
                        return true;
                    }
                }
            }

            // Agregado para Mapear Controles con entidades que son propiedades
            if (prop.PropertyType.Namespace.Contains("Entidades"))
            {
                object objetoInterno = prop.GetValue(pObjeto, null);
                Type tipoObjetoInterno = objetoInterno.GetType();
                PropertyInfo[] propiedadesInterno = tipoObjetoInterno.GetProperties();
                foreach (PropertyInfo propInterna in propiedadesInterno)
                {
                    if (MapearControlesEntidad(prop.Name, ctrl, objetoInterno, propInterna))
                        break;
                }
            }
            return false;
        }

        /// <summary>
        /// Establece los valores de las propiedad del obj1 en el obj2
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        public static void MatchObjectProperties(object obj1, object obj2)
        {
            Type t1 = obj1.GetType();
            Type t2 = obj2.GetType();
            PropertyInfo[] prop1 = t1.GetProperties();
            PropertyInfo[] prop2 = t2.GetProperties();

            BindingFlags flagsSet = BindingFlags.SetProperty;
            BindingFlags flagsGet = BindingFlags.GetProperty;

            Binder binder = null;
            object[] argsGet = null;
            object[] argsSet;

            object result;

            foreach (PropertyInfo p1 in prop1)
            {
                foreach (PropertyInfo p2 in prop2)
                {
                    if (p1.Name == p2.Name && p1.PropertyType == p2.PropertyType)
                    {
                        if (p1.Name.Contains("Entidades"))
                            MatchObjectProperties(p1.GetValue(obj1, null), p2.GetValue(obj2, null));
                        else
                        {
                            result = t1.InvokeMember(p1.Name, flagsGet, binder, obj1, argsGet);
                            if (result != null)
                            {
                                argsSet = new object[] { result };
                                t2.InvokeMember(p2.Name, flagsSet, binder, obj2, argsSet);
                            }
                        }
                        //result = t1.InvokeMember(p1.Name, flagsGet, binder, obj1, argsGet);
                        //if (result != null)
                        //{
                        //    argsSet = new object[] { result };
                        //    t2.InvokeMember(p2.Name, flagsSet, binder, obj2, argsSet);
                        //}
                    }
                }
            }
        }

        private static object ObtenerValor(Control pControl)
        {
            if (pControl is TextBox)
                return ((TextBox)pControl).Text;
            else if (pControl is CheckBox)
            {
                if (pControl.ID == "chkBajaLogica")
                    return !((CheckBox)pControl).Checked;
                else
                    return ((CheckBox)pControl).Checked;
            }
            else if (pControl is DropDownList)
                return ((DropDownList)pControl).SelectedValue;
            else
                return null;
        }

        public static List<Objeto> AcomodarIndices(List<Objeto> pLista)
        {
            foreach (Objeto obj in pLista)
            {
                obj.IndiceColeccion = pLista.IndexOf(obj);
            }
            return pLista;
        }

        public static List<T> AcomodarIndices<T>(List<T> pLista)
        {
            Type tipo;
            object[] indice;
            foreach (T obj in pLista)
            {
                tipo = obj.GetType();
                indice = new object[] { pLista.IndexOf(obj) };
                tipo.InvokeMember("IndiceColeccion", BindingFlags.SetProperty, null, obj, indice);
            }
            return pLista;
        }

        /// <summary>
        /// Reacomoda los indices de una coleeción y le asigna a cada Entidad su nuevo indice
        /// </summary>
        /// <param name="pLista"></param>
        public static List<Objeto> EliminarObjetoEnLista(List<Objeto> pLista)
        {
            pLista = pLista.FindAll(delegate(Objeto pObjeto)
            { return pObjeto.EstadoColeccion != EstadoColecciones.AgregadoBorradoMemoria; });

            foreach (Objeto obj in pLista)
            {
                obj.IndiceColeccion = pLista.IndexOf(obj);
            }
            return pLista;
        }

        /// <summary>
        /// Devuelve el Estado de un ítem en la colección dependiendo la acción
        /// que se esta realizando
        /// </summary>
        /// <param name="pObjetco"></param>
        /// <param name="pGestion"></param>
        /// <returns></returns>
        public static EstadoColecciones ObtenerEstadoColeccion(Objeto pObjetco, Gestion pGestion)
        {
            EstadoColecciones resultado = EstadoColecciones.SinCambio;
            switch (pGestion)
            {
                case Gestion.Consultar:
                    resultado = pObjetco.EstadoColeccion;
                    break;
                case Gestion.Agregar:
                    switch (pObjetco.EstadoColeccion)
                    {
                        case EstadoColecciones.SinCambio:
                            resultado = EstadoColecciones.Agregado;
                            break;
                        case EstadoColecciones.Agregado:
                            resultado = EstadoColecciones.Agregado;
                            break;
                        case EstadoColecciones.AgregadoBorradoMemoria:
                            //Lo utilizo para deshacer un Borrado en Memoria, en gral. en un objeto de Relacion
                            resultado = EstadoColecciones.Agregado;
                            break;
                        case EstadoColecciones.AgregadoPrevio:
                            resultado = EstadoColecciones.Agregado;
                            break;
                        case EstadoColecciones.Borrado:
                            //Lo utilizo para deshacer un Borrar, en gral. en un objeto Relación
                            resultado = EstadoColecciones.SinCambio;
                            break;
                        case EstadoColecciones.Modificado:
                            break;
                        default:
                            break;
                    }
                    break;
                case Gestion.Modificar:
                    switch (pObjetco.EstadoColeccion)
                    {
                        case EstadoColecciones.SinCambio:
                            //si lo traigo de la base y lo modifico
                            resultado = EstadoColecciones.Modificado;
                            break;
                        case EstadoColecciones.Agregado:
                            // si lo agrego y lo modifico queda como agregado
                            resultado = EstadoColecciones.Agregado;
                            break;
                        case EstadoColecciones.AgregadoPrevio:
                            resultado = EstadoColecciones.Agregado;
                            break;
                        case EstadoColecciones.Borrado:
                            // nunca se tendria que dar
                            resultado = EstadoColecciones.Modificado;
                            break;
                        case EstadoColecciones.Modificado:
                            //si es traido de la base de datos y es modificado
                            resultado = EstadoColecciones.Modificado;
                            break;
                        default:
                            break;
                    }
                    break;
                case Gestion.Anular:
                    switch (pObjetco.EstadoColeccion)
                    {
                        case EstadoColecciones.SinCambio:
                            //si viene de la base de datos lo borro, en la base de datos lo ponto como bajalogica true
                            resultado = EstadoColecciones.Borrado;
                            break;
                        case EstadoColecciones.Agregado:
                            //si lo agregue en memoria cuando persisto este ni lo persisto
                            resultado = EstadoColecciones.AgregadoBorradoMemoria;
                            break;
                        case EstadoColecciones.AgregadoPrevio:
                            resultado = EstadoColecciones.AgregadoBorradoMemoria;
                            break;
                        case EstadoColecciones.Borrado:
                            //esto nunca se tendria que dar
                            resultado = EstadoColecciones.Borrado;
                            break;
                        case EstadoColecciones.Modificado:
                            //si viene de la base de datos y lo modifique, y despues lo borre le pongo baja lojica true
                            resultado = EstadoColecciones.Borrado;
                            break;
                        default:
                            break;
                    }
                    break;
                case Gestion.Autorizar:
                    switch (pObjetco.EstadoColeccion)
                    {
                        case EstadoColecciones.SinCambio:
                            //si lo traigo de la base y lo modifico
                            resultado = EstadoColecciones.Modificado;
                            break;
                        case EstadoColecciones.Agregado:
                            // si lo agrego y lo modifico queda como agregado
                            resultado = EstadoColecciones.Agregado;
                            break;
                        case EstadoColecciones.AgregadoPrevio:
                            resultado = EstadoColecciones.Agregado;
                            break;
                        case EstadoColecciones.Borrado:
                            // nunca se tendria que dar
                            resultado = EstadoColecciones.Modificado;
                            break;
                        case EstadoColecciones.Modificado:
                            //si es traido de la base de datos y es modificado
                            resultado = EstadoColecciones.Modificado;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            return resultado;
        }

        /// <summary>
        /// Devuelve el Estado de un ítem en la colección dependiendo la acción
        /// que se esta realizando
        /// </summary>
        /// <param name="pObjetco"></param>
        /// <param name="pGestion"></param>
        /// <returns></returns>
        public static EstadoColecciones ObtenerEstadoColeccion(Menues pObjetco, Gestion pGestion)
        {
            EstadoColecciones resultado = EstadoColecciones.SinCambio;
            switch (pGestion)
            {
                case Gestion.Consultar:
                    resultado = pObjetco.EstadoColeccion;
                    break;
                case Gestion.Agregar:
                    switch (pObjetco.EstadoColeccion)
                    {
                        case EstadoColecciones.SinCambio:
                            resultado = EstadoColecciones.Agregado;
                            break;
                        case EstadoColecciones.Agregado:
                            resultado = EstadoColecciones.Agregado;
                            break;
                        case EstadoColecciones.AgregadoBorradoMemoria:
                            //Lo utilizo para deshacer un Borrado en Memoria, en gral. en un objeto de Relacion
                            resultado = EstadoColecciones.Agregado;
                            break;
                        case EstadoColecciones.AgregadoPrevio:
                            resultado = EstadoColecciones.Agregado;
                            break;
                        case EstadoColecciones.Borrado:
                            //Lo utilizo para deshacer un Borrar, en gral. en un objeto Relación
                            resultado = EstadoColecciones.SinCambio;
                            break;
                        case EstadoColecciones.Modificado:
                            break;
                        default:
                            break;
                    }
                    break;
                case Gestion.Modificar:
                    switch (pObjetco.EstadoColeccion)
                    {
                        case EstadoColecciones.SinCambio:
                            //si lo traigo de la base y lo modifico
                            resultado = EstadoColecciones.Modificado;
                            break;
                        case EstadoColecciones.Agregado:
                            // si lo agrego y lo modifico queda como agregado
                            resultado = EstadoColecciones.Agregado;
                            break;
                        case EstadoColecciones.AgregadoPrevio:
                            resultado = EstadoColecciones.Agregado;
                            break;
                        case EstadoColecciones.Borrado:
                            // nunca se tendria que dar
                            resultado = EstadoColecciones.Modificado;
                            break;
                        case EstadoColecciones.Modificado:
                            //si es traido de la base de datos y es modificado
                            resultado = EstadoColecciones.Modificado;
                            break;
                        default:
                            break;
                    }
                    break;
                case Gestion.Anular:
                    switch (pObjetco.EstadoColeccion)
                    {
                        case EstadoColecciones.SinCambio:
                            //si viene de la base de datos lo borro, en la base de datos lo ponto como bajalogica true
                            resultado = EstadoColecciones.Borrado;
                            break;
                        case EstadoColecciones.Agregado:
                            //si lo agregue en memoria cuando persisto este ni lo persisto
                            resultado = EstadoColecciones.AgregadoBorradoMemoria;
                            break;
                        case EstadoColecciones.AgregadoPrevio:
                            resultado = EstadoColecciones.AgregadoBorradoMemoria;
                            break;
                        case EstadoColecciones.Borrado:
                            //esto nunca se tendria que dar
                            resultado = EstadoColecciones.Borrado;
                            break;
                        case EstadoColecciones.Modificado:
                            //si viene de la base de datos y lo modifique, y despues lo borre le pongo baja lojica true
                            resultado = EstadoColecciones.Borrado;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            return resultado;
        }

        /// <summary>
        /// Carga una grilla de datos
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pLista"></param>
        /// <param name="pGrilla"></param>
        /// <param name="pMostrarCabecera"></param>
        public static void CargarGrillaListas<T>(List<T> pLista, bool pFiltrarEstadoColeccion, GridView pGrilla, bool pMostrarCabecera) where T : new()
        {
            if (pFiltrarEstadoColeccion)
            {
                pLista = pLista.Cast<Objeto>().Where(x => x.EstadoColeccion == EstadoColecciones.SinCambio
                        || x.EstadoColeccion == EstadoColecciones.Agregado
                        || x.EstadoColeccion == EstadoColecciones.Modificado).Cast<T>().ToList();
            }

            pGrilla.ShowHeaderWhenEmpty = true;
            pGrilla.DataSource = pLista;
            pGrilla.DataBind();
            FixGridView(pGrilla);
            //if (pMostrarCabecera && pLista.Count == 0)
            //{
            //    T item = new T();
            //    List<T> lista = new List<T>();
            //    Type tipo = item.GetType();
            //    lista.Add(item);
            //    pGrilla.DataSource = lista;
            //    pGrilla.DataBind();
            //    pGrilla.ShowHeaderWhenEmpty = true;
            //}
            //else
            //{
            //    pGrilla.DataSource = pLista;
            //    pGrilla.DataBind();
            //}
        }

        public static void FixGridView(GridView gv)
        {
            if ((gv.ShowHeader == true && gv.Rows.Count > 0)
                || (gv.ShowHeaderWhenEmpty == true))
            {
                if(gv.HeaderRow != null)
                    gv.HeaderRow.TableSection = TableRowSection.TableHeader;
                gv.UseAccessibleHeader = true;
            }
            //if (gv.ShowFooter == true && gv.Rows.Count > 0)
            //{
            //    Force GridView to use<tfoot> instead of < tbody > -11 / 03 / 2013 - MCR.
            //   gv.FooterRow.TableSection = TableRowSection.TableFooter;
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valores"></param>
        /// <param name="pDir"></param>
        /// <param name="pNombre"></param>
        /// <returns></returns>
        public static Hashtable ObtenerOrdenamientosGrillas(Hashtable valores, SortDirection pDir, string pNombre)
        {
            if (valores.ContainsKey(pNombre))
            {
                SortDirection direccion = (SortDirection)valores[pNombre];
                if (direccion == SortDirection.Ascending)
                    valores[pNombre] = SortDirection.Descending;
                else
                    valores[pNombre] = SortDirection.Ascending;
            }
            else
            {
                valores = new Hashtable();
                valores.Add(pNombre, pDir);
            }

            return valores;
        }

        ///// <summary>
        ///// Devuelve el nombre del Crystal Report para mostrar segun el Ambiente de ejecucion
        ///// </summary>
        ///// <param name="pReporte"></param>
        ///// <returns></returns>
        //public static string ObtenerNombreReporte(string pReporte)
        //{
        //    return string.Concat(pReporte, TGEGeneralesF.TGEParametrosObtenerUno(EnumTGEParametros.ReportEnvironment).ValorParametros.Trim());
        //}

        public static UsuarioLogueado ObtenerDatosUsuario(Usuarios pUsuarioActivo)
        {
            UsuarioLogueado usuario = new UsuarioLogueado();
            MatchObjectProperties(pUsuarioActivo, usuario);
            usuario.IdFilialEvento = pUsuarioActivo.FilialPredeterminada.IdFilial;
            usuario.ConsultarAuditoria = pUsuarioActivo.Menues.Exists(x => x.URL == "AuditoriaDatos");
            return usuario;
        }

        /// <summary>
        /// Devuelve la ubicacion del Sistema segun el MENU
        /// </summary>
        /// <param name="pMenu"></param>
        /// <param name="pUsuario"></param>
        /// <param name="pUbicacion"></param>
        /// <returns></returns>
        public static string ObtenerUbicacion(Menues pMenu, Usuarios pUsuario, string pUbicacion)
        {
            if (pMenu.IdMenuPadre > 0 && pUsuario.Menues.Exists(x => x.IdMenu == pMenu.IdMenuPadre))
            {
                pUbicacion = string.Concat(pUbicacion, ObtenerUbicacion(pUsuario.Menues.Find(x => x.IdMenu == pMenu.IdMenuPadre), pUsuario, pUbicacion));
            }
            pUbicacion = string.Concat(pUbicacion, " >> ", pMenu.Menu);

            return pUbicacion;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pFecha"></param>
        /// <returns>ShortDateString || string.Empty</returns>
        public static string MostrarFechaPantalla(DateTime pFecha)
        {
            if (pFecha < Convert.ToDateTime("1800/01/01"))
                return string.Empty;
            else
                return pFecha.ToShortDateString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pFecha"></param>
        /// <returns>ShortDateString || string.Empty</returns>
        public static string MostrarFechaPantalla(DateTime? pFecha)
        {
            if (!pFecha.HasValue)
                return string.Empty;
            else
            {
                if (pFecha < Convert.ToDateTime("1800/01/01"))
                    return string.Empty;

                return pFecha.Value.ToShortDateString();
            }
        }

        /// <summary>
        /// Asigna la clase "EstadoMoroso" de la hoja de Estilo al control segun el estado
        /// </summary>
        /// <param name="pControl"></param>
        /// <param name="pAfiliado"></param>
        public static void FormatoEstadoSocio(TextBox pControl, AfiAfiliados pAfiliado)
        {
            if (pAfiliado.Estado.IdEstado == (int)EstadosAfiliados.Moroso)
                pControl.CssClass = "EstadoMoroso";
        }

        public static void FormatoResaltado(TextBox pControl)
        {
            pControl.CssClass = "EstadoMoroso";
        }

        /// <summary>
        /// Devuelve el Periodo de una Fecha
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static int ObtenerPeriodo(DateTime pParametro)
        {
            string anio = pParametro.Year.ToString();
            string mes = pParametro.Month.ToString().PadLeft(2, '0');
            return Convert.ToInt32(string.Concat(anio, mes));
        }

        public static int CalcularEdad(DateTime? fechaNacimiento)
        {
            if(!fechaNacimiento.HasValue)
                return 0;
            if (fechaNacimiento <= DateTime.MinValue)
                return 0;
            // Obtiene la fecha actual:
            DateTime fechaActual = DateTime.Today;
            
            // Comprueba que la se haya introducido una fecha válida; si 
            // la fecha de nacimiento es mayor a la fecha actual se muestra mensaje 
            // de advertencia:
            if (fechaNacimiento > fechaActual)
            {
                //Console.WriteLine ("La fecha de nacimiento es mayor que la actual.");
                return -1;
            }
            else 
            {
                int edad = fechaActual.Year - fechaNacimiento.Value.Year;
                
                // Comprueba que el mes de la fecha de nacimiento es mayor 
                // que el mes de la fecha actual:
                if (fechaNacimiento.Value.Month > fechaActual.Month)
                {
                    --edad;
                }
                
                return edad;
            }
        }

        /// <summary>
        /// Ordena un TreView
        /// </summary>
        /// <param name="treeNodes"></param>
        public static void SortTreeNodes(TreeNodeCollection treeNodes)
        {
            var sorted = true;

            foreach (TreeNode treeNode in treeNodes)
            {
                SortTreeNodes(treeNode.ChildNodes);
            }

            do
            {
                sorted = true;

                for (var i = 0; i < treeNodes.Count - 1; i++)
                {
                    var treeNode1 = treeNodes[i];
                    var treeNode2 = treeNodes[i + 1];

                    if (treeNode1.Text.CompareTo(treeNode2.Text) > 0)
                    {
                        treeNodes.RemoveAt(i + 1);
                        treeNodes.RemoveAt(i);

                        treeNodes.AddAt(i, treeNode2);
                        treeNodes.AddAt(i + 1, treeNode1);

                        sorted = false;
                    }
                }
            } while (!sorted);
        }

        public static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string ObtenerUrlParametros(string url)
        {
            string queryString = HttpContext.Current.Request.QueryString.ToString();
            return string.Concat(url, "?", queryString);
        }

        public static DataTable PivotList(List<TGECampos> lista)
        {
            DataTable dt = new DataTable();
            foreach (TGECampos item in lista)
                dt.Columns.Add(item.Titulo);

            DataRow row = dt.NewRow();
            foreach (TGECampos item in lista)
                switch (item.CampoTipo.IdCampoTipo)
                {
                    case (int)EnumCamposTipos.DropDownList:
                    case (int)EnumCamposTipos.DropDownListQuery:
                    case (int)EnumCamposTipos.DropDownListSP:
                    case (int)EnumCamposTipos.DropDownListSPAutoComplete:
                    case (int)EnumCamposTipos.ComboBoxSP:
                        row[item.Titulo] = item.CampoValor.ListaValor;
                        break;
                    default:
                        row[item.Titulo] = item.CampoValor.Valor;
                        break;
                }
            dt.Rows.Add(row);

            return dt;
        }

        public static int GridviewIndexColumn(GridView gv, string HeaderName)
        {
            int result = -1;
            for (int i = 0; i < gv.Columns.Count; i++)
            {
                if (gv.Columns[i].HeaderText == HeaderName)
                {
                    gv.Columns[i].Visible = false;
                    result = i;
                    break;
                }
            }
            return result;
        }

        public static int ObtenerGridviewPageSize(int pageSize)
        {
            if (pageSize == 0)
                return 25; //Valor por defecto

            int result = 0;
            List<int> valores = new List<int>();
            valores.Add(10);
            valores.Add(25);
            valores.Add(50);
            valores.Add(100);
            valores.Add(500);
            valores.Add(1000);
            valores.Add(5000);
            valores.Add(10000);
            valores.Add(100000);
            valores.Add(1000000);
            valores.Add(100000000);
            if (valores.Exists(v => v == pageSize))
                result = pageSize;
            else
                result = valores.Where(v => v > pageSize).Min();
            return result;
        }

        public static Objeto ObtenerIdTipoOperacion(TESCajasMovimientos pParametro)
        {
            Objeto objeto = new Objeto();
            switch (pParametro.TipoOperacion.IdTipoOperacion)
            {
                case (int)EnumTGETiposOperaciones.OrdenesPagos:
                case (int)EnumTGETiposOperaciones.OrdenesPagosInterno:

                    objeto = new CapOrdenesPagos();
                    ((CapOrdenesPagos)objeto).IdOrdenPago = pParametro.IdRefTipoOperacion;
                    break;
                case (int)EnumTGETiposOperaciones.OrdenesCobrosFacturas:
                case (int)EnumTGETiposOperaciones.OrdenesCobrosFacturasInternas:
                case (int)EnumTGETiposOperaciones.OrdenesCobrosAfiliados:
                case (int)EnumTGETiposOperaciones.CancelacionAnticipadaCuotaPrestamo:

                    objeto = new CobOrdenesCobros();
                    ((CobOrdenesCobros)objeto).IdOrdenCobro = pParametro.IdRefTipoOperacion;
                    break;
                case (int)EnumTGETiposOperaciones.AhorroExtracciones:
                //case (int)EnumTGETiposOperaciones.AhorroDepositos:

                //    objeto = new CobOrdenesCobros();
                //    ((CobOrdenesCobros)objeto).IdOrdenCobro = pParametro.IdRefTipoOperacion;
                //    break;
                case (int)EnumTGETiposOperaciones.PrestamosLargoPlazo:
                case (int)EnumTGETiposOperaciones.PrestamosCortoPlazo:
                case (int)EnumTGETiposOperaciones.PrestamosFondosPropios:
                case (int)EnumTGETiposOperaciones.PrestamosFondosPropiosCancelacion:
                case (int)EnumTGETiposOperaciones.PrestamosLargoPlazoCancelacion:
                case (int)EnumTGETiposOperaciones.PrestamosCortoPlazoCancelacion:
                case (int)EnumTGETiposOperaciones.Prestamos46:
                case (int)EnumTGETiposOperaciones.Prestamos49:
                case (int)EnumTGETiposOperaciones.Prestamos50:
                case (int)EnumTGETiposOperaciones.PrestamosManual:
                case (int)EnumTGETiposOperaciones.PrestamosBancoDelSol:
                case (int)EnumTGETiposOperaciones.PrestamosBancoDelSolCancelacion:
                case (int)EnumTGETiposOperaciones.CompraDeCheque:


                    objeto = new PrePrestamos();
                    ((PrePrestamos)objeto).IdPrestamo = pParametro.IdRefTipoOperacion;
                    break;
                //case (int)EnumTGETiposOperaciones.ExtraccionCheque:

                //    objeto = new CobOrdenesCobros();
                //    ((CobOrdenesCobros)objeto).IdOrdenCobro = pParametro.IdRefTipoOperacion;
                //    break;
            }
            return objeto;
        }



    }
}