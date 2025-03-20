#nullable enable

using System;
using System.Collections.Generic;

namespace Beton.Core.Features
{
    public class FeaturesStorage : IDisposable
    {
        private readonly Dictionary<Type, IFeature> _features = new();
        
        public TFeature GetFeature<TFeature>()
            where TFeature : IFeature, new()
        {
            if (_features.TryGetValue(typeof(TFeature), out IFeature featureBase))
            {
                var feature = (TFeature)featureBase;
                return feature;
            }

            var newFeature = new TFeature();
            _features.Add(typeof(TFeature), newFeature);
            return newFeature;
        }
        
        public void DisposeFeature<TFeature>()
            where TFeature : IFeature
        {
            if (_features.TryGetValue(typeof(TFeature), out IFeature featureBase))
            {
                featureBase.Dispose();
                _features.Remove(typeof(TFeature));
            }
        }
        
        public void Dispose()
        {
            foreach (var feature in _features.Values)
            {
                feature.Dispose();
            }
            
            _features.Clear();
        }
    }
}