namespace project_cms.DTOs
{
    /// <summary>
    /// DTO de réponse pour exposer les données d'un article au client.
    /// Contient les champs lisibles en sortie (id, titre, contenu, dates).
    /// </summary>
    public class ArticleResponseDTO
    {
        /// <summary>Identifiant unique de l'article.</summary>
        public int Id { get; set; }

        /// <summary>Titre de l'article.</summary>
        public string Title { get; set; }

        /// <summary>Contenu texte de l'article.</summary>
        public string Content { get; set; }

        /// <summary>Date de publication (UTC).</summary>
        public DateTime PublishedDate { get; set; }

        /// <summary>Date de dernière mise à jour (UTC).</summary>
        public DateTime UpdatedDate { get; set; }
    }
}
