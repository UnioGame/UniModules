using UniGreenModules.UniGame.UiSystem.Runtime.Settings;
using UnityEngine;

namespace UniGreenModules.UniGame.UiSystem.Examples.ListViews
{
    using Runtime;
    using UniCore.Runtime.ProfilerTools;
    using UniRx;
    using ViewModels;
    using Views;

    public class DemoStartListView : MonoBehaviour
    {
        public GameViewSystemComponent viewSystem;
    
        public DemoItemViewModel demoItemViewModel = new DemoItemViewModel();
    
        public DemoListViewModel listModel = new DemoListViewModel();
        
        // Start is called before the first frame update
        private async void Start()
        {
            var view = await viewSystem.OpenScreen<DemoListView>(listModel);

            listModel.AddTo(this);

            listModel.Add.
                Do(x => GameLog.Log($"ADD NEW Demo List Item")).
                Subscribe(x => 
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
