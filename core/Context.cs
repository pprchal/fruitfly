using fruitfly.objects;

namespace fruitfly
{
    public class Context
    {
        public Configuration Config
        {
            get;
            set;
        }

        private HtmlRenderer _Renderer  = null;
        public HtmlRenderer Renderer
        {
            get
            {
                if(_Renderer == null)
                {
                    _Renderer = new HtmlRenderer(this);
                }
                return _Renderer;
            }
        }

        private VariableBinder _Binder  = null;
        public VariableBinder Binder
        {
            get
            {
                if(_Binder == null)
                {
                    _Binder = new VariableBinder(this);
                }
                return _Binder;
            }
        }

        private BlogStorage _Storage  = null;
        public BlogStorage Storage
        {
            get
            {
                if(_Storage == null)
                {
                    _Storage = new BlogStorage(this);
                }
                return _Storage;
            }
        }
    }
}
