using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DeepEditor.Common
{
    public class UndoRedoManager
    {
        private int capacity;
        private LinkedList<Command> cmd_undo_list;
        private LinkedList<Command> cmd_redo_list;

        private Action event_OnListChanged;
        private Action<Command, Exception> event_OnExecuteError;

        public bool CanUndo { get { return cmd_undo_list.Count > 0; } }
        public bool CanRedo { get { return cmd_redo_list.Count > 0; } }
        public int UndoCount { get { return cmd_undo_list.Count; } }
        public int RedoCount { get { return cmd_redo_list.Count; } }

        public event Action OnListChanged
        {
            add { event_OnListChanged += value; }
            remove { event_OnListChanged -= value; }
        }
        public event Action<Command, Exception> OnExecuteError
        {
            add { event_OnExecuteError += value; }
            remove { event_OnExecuteError -= value; }
        }

        public UndoRedoManager(int capacity)
        {
            this.capacity = capacity;
            this.cmd_undo_list = new LinkedList<Command>();
            this.cmd_redo_list = new LinkedList<Command>();
        }
        public void BindButton(ToolStripButton btn_Redo, ToolStripButton btn_Undo)
        {
            btn_Redo.Click += (s, e) => { this.Redo(); };
            btn_Undo.Click += (s, e) => { this.Undo(); };
            btn_Undo.Enabled = this.CanUndo;
            btn_Redo.Enabled = this.CanRedo;
            this.event_OnListChanged += () =>
            {
                btn_Undo.Enabled = this.CanUndo;
                btn_Redo.Enabled = this.CanRedo;
            };
        }
        public void BindButton(ToolStripMenuItem btn_Redo, ToolStripMenuItem btn_Undo)
        {
            btn_Redo.Click += (s, e) => { this.Redo(); };
            btn_Undo.Click += (s, e) => { this.Undo(); };
            btn_Undo.Enabled = this.CanUndo;
            btn_Redo.Enabled = this.CanRedo;
            this.event_OnListChanged += () =>
            {
                btn_Undo.Enabled = this.CanUndo;
                btn_Redo.Enabled = this.CanRedo;
            };
        }

        public void Clear()
        {
            cmd_undo_list.Clear();
            cmd_redo_list.Clear();
            event_OnListChanged?.Invoke();
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
        public R ExecuteAs<T, R>(Func<T, R> exe, Action<T> undo, T data, string text = null)
        {
            return this.ExecuteAs<T, T, T, R>(exe, (r) => exe(r), undo, data, data, data, text);
        }
        public R ExecuteAs<T, R>(Func<T, R> exe, Action<T> redo, Action<T> undo, T data, string text = null)
        {
            return this.ExecuteAs<T, T, T, R>(exe, redo, undo, data, data, data, text);
        }
        public R ExecuteAs<T1, T2, R>(Func<T1, R> exe, Action<T2> undo, T1 redo_data, T2 undo_data, string text = null)
        {
            return this.ExecuteAs<T1, T1, T2, R>(exe, (r) => exe(r), undo, redo_data, redo_data, undo_data, text);
        }
        public R ExecuteAs<T1, T2, T3, R>(Func<T1, R> exe, Action<T2> redo, Action<T3> undo, T1 exe_data, T2 redo_data, T3 undo_data, string text = null)
        {
            return (R)Execute(
               new Func<object, object>(t1 => exe((T1)t1)),
               new Action<object>(t2 => redo((T2)t2)),
               new Action<object>(t3 => undo((T3)t3)),
               exe_data, redo_data, undo_data, text);
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
        public void ExecuteAs<T>(Action<T> exe, Action<T> undo, T data, string text = null)
        {
             this.ExecuteAs<T, T, T>(exe, exe, undo, data, data, data, text);
        }
        public void ExecuteAs<T>(Action<T> exe, Action<T> redo, Action<T> undo, T data, string text = null)
        {
             this.ExecuteAs<T, T, T>(exe, redo, undo, data, data, data, text);
        }
        public void ExecuteAs<T1, T2>(Action<T1> exe, Action<T2> undo, T1 redo_data, T2 undo_data, string text = null)
        {
             this.ExecuteAs<T1, T1, T2>(exe, exe, undo, redo_data, redo_data, undo_data, text);
        }
        public void ExecuteAs<T1, T2, T3>(Action<T1> exe, Action<T2> redo, Action<T3> undo, T1 exe_data, T2 redo_data, T3 undo_data, string text = null)
        {
             Execute(
               new Action<object>(t1 => exe((T1)t1)),
               new Action<object>(t2 => redo((T2)t2)),
               new Action<object>(t3 => undo((T3)t3)),
               exe_data, redo_data, undo_data, text);
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
        public void Execute(Action<object> exe, Action<object> undo, object data, string text = null)
        {
             this.Execute(exe, exe, undo, data, data, data, text);
        }
        public void Execute(Action<object> exe, Action<object> redo, Action<dynamic> undo, object data, string text = null)
        {
             this.Execute(exe, redo, undo, data, data, data, text);
        }
        public void Execute(Action<object> exe, Action<object> undo, object redo_data, object undo_data, string text = null)
        {
             this.Execute(exe, exe, undo, redo_data, redo_data, undo_data, text);
        }
        public void Execute(Action<object> exe, Action<object> redo, Action<object> undo, object exe_data, object redo_data, object undo_data, string text = null)
        {
            this.Execute(new Func<object, object>(r => { exe(r); return null; }), redo, undo, exe_data, redo_data, undo_data, text);
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
        public object Execute(Func<object, object> exe, Action<object> undo, object data, string text = null)
        {
            return this.Execute(exe, (r) => exe(r), undo, data, data, data, text);
        }
        public object Execute(Func<object, object> exe, Action<object> redo, Action<dynamic> undo, object data, string text = null)
        {
            return this.Execute(exe, redo, undo, data, data, data, text);
        }
        public object Execute(Func<object, object> exe, Action<object> undo, object redo_data, object undo_data, string text = null)
        {
            return this.Execute(exe, (r) => exe(r), undo, redo_data, redo_data, undo_data, text);
        }
        public object Execute(Func<object, object> exe, Action<object> redo, Action<object> undo, object exe_data, object redo_data, object undo_data, string text = null)
        {
            var cmd = new Command(exe, redo, undo, exe_data, redo_data, undo_data);
            object result = null;
            cmd.Text = text;
            try
            {
                result = cmd.DoExecute();
            }
            catch (Exception err)
            {
                event_OnExecuteError?.Invoke(cmd, err);
            }
            if (cmd_undo_list.Count > capacity)
            {
                cmd_undo_list.RemoveFirst();
            }
            cmd_redo_list.Clear();
            cmd_undo_list.AddLast(cmd);
            event_OnListChanged?.Invoke();
            return result;
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------









        public bool Undo()
        {
            if (cmd_undo_list.Count > 0)
            {
                var cmd = cmd_undo_list.Last();
                cmd_undo_list.RemoveLast();
                try { cmd.DoUndo(); } catch (Exception err) { event_OnExecuteError?.Invoke(cmd, err); }
                cmd_redo_list.AddLast(cmd);
                event_OnListChanged?.Invoke();
                return true;
            }
            return false;
        }
        public bool Redo()
        {
            if (cmd_redo_list.Count > 0)
            {
                var cmd = cmd_redo_list.Last();
                cmd_redo_list.RemoveLast();
                try { cmd.DoRedo(); } catch (Exception err) { event_OnExecuteError?.Invoke(cmd, err); }
                cmd_undo_list.AddLast(cmd);
                event_OnListChanged?.Invoke();
                return true;
            }
            return false;
        }
    }

    public class Command
    {
        public readonly object exe_data;
        public readonly object redo_data;
        public readonly object undo_data;
        public readonly Func<object, object> Execute;
        public readonly Action<object> Redo;
        public readonly Action<object> Undo;
        public string Text;

        internal Command(Func<object, object> exe, Action<object> redo, Action<object> undo, object edata, object rdata, object udata)
        {
            this.exe_data = edata;
            this.redo_data = rdata;
            this.undo_data = udata;
            this.Execute = exe;
            this.Redo = redo;
            this.Undo = undo;
        }
        internal object DoExecute()
        {
            return this.Execute(this.exe_data);
        }
        internal void DoRedo()
        {
            this.Redo(this.redo_data);
        }
        internal void DoUndo()
        {
            this.Undo(this.undo_data);
        }
    }
}
