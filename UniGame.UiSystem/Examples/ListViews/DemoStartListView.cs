using UniGreenModules.UniGame.UiSystem.Runtime.Settings;
using UnityEngine;

namespace UniGreenModules.UniGame.UiSystem.Examples.ListViews
{
    using Runtime;
    using UniRx;
    using ViewModels;
    using Views;

    public class DemoStartListView : MonoBehaviour
    {
        public GameViewSystem viewSystem;
    
        public DemoItemViewModel demoItemViewModel = new DemoItemViewModel();
    
        public DemoListViewModel listModel = new DemoListViewModel();
        
        // Start is called before the first frame update
        private async void Start()
        {
            var view = await viewSystem.OpenScreen<DemoListView>(listModel);

            listModel.AddTo(this);

            listModel.Add.Subscribe(x => 
                    listModel.ListItems.Add(new DemoItemViewModel() {
                        Armor = new IntReactiveProperty(demoItemViewModel.Armor.Value),
                        Damage = new IntReactiveProperty(demoItemViewModel.Damage.Value),
                        Level = new IntReactiveProperty(demoItemViewModel.Level.Value),
                        Icon = new ReactiveProperty<Sprite>(demoItemViewModel.Icon.Value),
                        Buy = new ReactiveCommand(),
                        Remove = new ReactiveCommand(),
                    })).AddTo(this);
            
            view.Show();

        }

    }
}
