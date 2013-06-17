// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.ModelConfiguration.Configuration
{
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Data.Entity.Edm;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Data.Entity.Utilities;

    public partial class ConventionsConfiguration
    {
        private class ModelConventionDispatcher : EdmModelVisitor
        {
            private readonly IConvention _convention;
            private readonly EdmModel _model;

            public ModelConventionDispatcher(IConvention convention, EdmModel model)
            {
                Check.NotNull(convention, "convention");
                Check.NotNull(model, "model");

                _convention = convention;
                _model = model;
            }

            public void Dispatch()
            {
                VisitEdmModel(_model);
            }

            private void Dispatch<TEdmDataModelItem>(TEdmDataModelItem item)
                where TEdmDataModelItem : MetadataItem
            {
                var convention = _convention as IModelConvention<TEdmDataModelItem>;

                if (convention != null)
                {
                    convention.Apply(item, _model);
                }
            }

            protected internal override void VisitEdmModel(EdmModel item)
            {
                var convention = _convention as IModelConvention;

                if (convention != null)
                {
                    convention.Apply(item);
                }

                base.VisitEdmModel(item);
            }

            protected override void VisitEdmNavigationProperty(NavigationProperty item)
            {
                Dispatch(item);

                base.VisitEdmNavigationProperty(item);
            }

            protected override void VisitEdmAssociationConstraint(ReferentialConstraint item)
            {
                Dispatch(item);

                if (item != null)
                {
                    VisitMetadataItem(item);
                }
            }

            protected override void VisitEdmAssociationEnd(RelationshipEndMember item)
            {
                Dispatch(item);

                base.VisitEdmAssociationEnd(item);
            }

            protected internal override void VisitEdmProperty(EdmProperty item)
            {
                Dispatch(item);

                base.VisitEdmProperty(item);
            }

            protected internal override void VisitMetadataItem(MetadataItem item)
            {
                Dispatch(item);

                base.VisitMetadataItem(item);
            }

            protected override void VisitEdmEntityContainer(EntityContainer item)
            {
                Dispatch(item);

                base.VisitEdmEntityContainer(item);
            }

            protected internal override void VisitEdmEntitySet(EntitySet item)
            {
                Dispatch(item);

                base.VisitEdmEntitySet(item);
            }

            protected override void VisitEdmAssociationSet(AssociationSet item)
            {
                Dispatch(item);

                base.VisitEdmAssociationSet(item);
            }

            protected override void VisitEdmAssociationSetEnd(EntitySet item)
            {
                Dispatch(item);

                base.VisitEdmAssociationSetEnd(item);
            }

            protected override void VisitComplexType(ComplexType item)
            {
                Dispatch(item);

                base.VisitComplexType(item);
            }

            protected internal override void VisitEdmEntityType(EntityType item)
            {
                Dispatch(item);

                VisitMetadataItem(item);

                if (item != null)
                {
                    VisitDeclaredProperties(item, item.DeclaredProperties);
                    VisitDeclaredNavigationProperties(item, item.DeclaredNavigationProperties);
                }
            }

            protected internal override void VisitEdmAssociationType(AssociationType item)
            {
                Dispatch(item);

                base.VisitEdmAssociationType(item);
            }
        }
    }
}