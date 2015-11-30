﻿using DD4T.ContentModel;
using DD4T.Templates.Base;
using DD4T.Templates.Base.Utils;
using System;
using Tridion.ContentManager.Templating;
using Tridion.ContentManager.Templating.Assembly;
using Dynamic = DD4T.ContentModel;

namespace DD4T.Templates
{
    [Obsolete("Binaries are published by the default DynamicPage template")]
    [TcmTemplateTitle("Publish binaries for page")]
    [TcmTemplateParameterSchema("resource:DD4T.Templates.Resources.Schemas.Dynamic Delivery Parameters.xsd")]
    public class PublishBinariesPage : BasePageTemplate
    {
        private BinaryPublisher _binaryPublisher = null;
        protected BinaryPublisher BinaryPublisher
        {
            get
            {
                if (_binaryPublisher == null)
                {
                    _binaryPublisher = new BinaryPublisher(Package, Engine);
                }
                return _binaryPublisher;
            }
        }

        public PublishBinariesPage() : base(TemplatingLogger.GetLogger(typeof(PublishBinariesPage))) { }
        public PublishBinariesPage(TemplatingLogger log) : base(log) { }

        protected override void TransformPage(Dynamic.Page page)
        {
            // call helper function to publish all relevant multimedia components
            // Note: this template only published mm components that are found in the metadata of the page
            // MM components that are used as component presentation, or that are linked from a component that is 
            // used as a component presentation, will be published from the component template
            // (e.g. by adding 'Publish binaries for components' to that CT)

            PublishAllBinaries(page);
        }

        private void PublishAllBinaries(Dynamic.Page page)
        {

            foreach (Dynamic.Field field in page.MetadataFields.Values)
            {
                if (field.FieldType == Dynamic.FieldType.ComponentLink || field.FieldType == Dynamic.FieldType.MultiMediaLink)
                {
                    foreach (Dynamic.Component linkedComponent in field.LinkedComponentValues)
                    {
                        PublishAllBinaries(linkedComponent);
                    }
                }
                if (field.FieldType == Dynamic.FieldType.Embedded)
                {
                    foreach (Dynamic.FieldSet embeddedFields in field.EmbeddedValues)
                    {
                        PublishAllBinaries(embeddedFields);
                    }
                }
                if (field.FieldType == Dynamic.FieldType.Xhtml)
                {
                    for (int i = 0; i < field.Values.Count; i++)
                    {
                        string xhtml = field.Values[i];
                        field.Values[i] = BinaryPublisher.PublishBinariesInRichTextField(xhtml, Manager.BuildProperties);
                    }
                }
            }

        }

        protected void PublishAllBinaries(Dynamic.FieldSet fieldSet)
        {
            foreach (Dynamic.Field field in fieldSet.Values)
            {
                if (field.FieldType == Dynamic.FieldType.ComponentLink || field.FieldType == Dynamic.FieldType.MultiMediaLink)
                {
                    foreach (Dynamic.Component linkedComponent in field.LinkedComponentValues)
                    {
                        PublishAllBinaries(linkedComponent);
                    }
                }
                if (field.FieldType == Dynamic.FieldType.Embedded)
                {
                    foreach (Dynamic.FieldSet embeddedFields in field.EmbeddedValues)
                    {
                        PublishAllBinaries(embeddedFields);
                    }
                }
                if (field.FieldType == Dynamic.FieldType.Xhtml)
                {
                    for (int i = 0; i < field.Values.Count; i++)
                    {
                        string xhtml = field.Values[i];
                        field.Values[i] = BinaryPublisher.PublishBinariesInRichTextField(xhtml, Manager.BuildProperties);
                    }
                }
            }
        }


        private void PublishAllBinaries(Dynamic.Component component)
        {
            if (component.ComponentType.Equals(Dynamic.ComponentType.Multimedia))
            {
                BinaryPublisher.PublishMultimediaComponent(component, Manager.BuildProperties);
            }
            foreach (var field in component.Fields.Values)
            {
                if (field.FieldType == Dynamic.FieldType.ComponentLink || field.FieldType == Dynamic.FieldType.MultiMediaLink)
                {
                    foreach (IComponent linkedComponent in field.LinkedComponentValues)
                    {
                        PublishAllBinaries(linkedComponent as Component);
                    }
                }
                if (field.FieldType == Dynamic.FieldType.Embedded)
                {
                    foreach (Dynamic.FieldSet embeddedFields in field.EmbeddedValues)
                    {
                        PublishAllBinaries(embeddedFields);
                    }
                }
                if (field.FieldType == Dynamic.FieldType.Xhtml)
                {
                    for (int i = 0; i < field.Values.Count; i++)
                    {
                        string xhtml = field.Values[i];
                        field.Values[i] = BinaryPublisher.PublishBinariesInRichTextField(xhtml, Manager.BuildProperties);
                    }
                }
            }
            foreach (var field in component.MetadataFields.Values)
            {
                if (field.FieldType == Dynamic.FieldType.ComponentLink || field.FieldType == Dynamic.FieldType.MultiMediaLink)
                {
                    foreach (Dynamic.Component linkedComponent in field.LinkedComponentValues)
                    {
                        PublishAllBinaries(linkedComponent);
                    }
                }
                if (field.FieldType == Dynamic.FieldType.Xhtml)
                {
                    for (int i = 0; i < field.Values.Count; i++)
                    {
                        string xhtml = field.Values[i];
                        field.Values[i] = BinaryPublisher.PublishBinariesInRichTextField(xhtml, Manager.BuildProperties);
                    }
                }
            }
        }
    }
}