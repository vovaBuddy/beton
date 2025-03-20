#nullable enable

using System;
using System.Collections.Generic;
using Beton.Core.DependencyInjections;
using Beton.Core.Features;

namespace Beton.Core.GameStates
{
    public abstract class GameState : IDisposable
    {
        public IContext Context { get; }
        public List<Feature> Features { get; }
        public List<Initializer> Initializers { get; }
        
        private List<ITickFeature> _tickFeatures;
        private List<ILateTickFeature> _lateTickFeatures;
        private List<IFixedTickFeature> _fixedTickFeatures;
        
        private readonly FeaturesStorage _featuresStorage;
        private readonly HashSet<Type> _featuresTypes;
        
        protected GameState(IContext globalContext, FeaturesStorage featuresStorage)
        {
            Context = new Context(globalContext);
            
            Features = new List<Feature>();
            Initializers = new List<Initializer>();
            
            _tickFeatures = new List<ITickFeature>();
            _lateTickFeatures = new List<ILateTickFeature>();
            _fixedTickFeatures = new List<IFixedTickFeature>();
            
            _featuresTypes = new HashSet<Type>();
            
            _featuresStorage = featuresStorage;
        }
        
        protected void AddFeature<TFeature>()
            where TFeature : IFeature, new()
        {
            var feature = _featuresStorage.GetFeature<TFeature>();
            
            _featuresTypes.Add(typeof(TFeature));
            
            if (feature is Initializer initializer)
            {
                Initializers.Add(initializer);
            }
            
            if (feature is Feature featureBase)
            {
                Features.Add(featureBase);
            }
            
            if (feature is ITickFeature tickFeature)
            {
                _tickFeatures.Add(tickFeature);
            }
            
            if (feature is ILateTickFeature lateTickFeature)
            {
                _lateTickFeatures.Add(lateTickFeature);
            }
            
            if (feature is IFixedTickFeature fixedTickFeature)
            {
                _fixedTickFeatures.Add(fixedTickFeature);
            }
        }
        
        public bool HasFeature(Type type)
        {
            return _featuresTypes.Contains(type);
        }

        public void Tick(float deltaTime)
        {
            foreach (var feature in _tickFeatures)
            {
                feature.Tick(deltaTime);
            }
        }
        
        public void LateTick(float deltaTime)
        {
            foreach (var feature in _lateTickFeatures)
            {
                feature.Tick(deltaTime);
            }
        }
        
        public void FixedTick(float deltaTime)
        {
            foreach (var feature in _fixedTickFeatures)
            {
                feature.Tick(deltaTime);
            }
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}