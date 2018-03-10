using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MoreForLess.BusinessLogic.Models;
using MoreForLess.BusinessLogic.Services.Interfaces;
using MoreForLess.DataAccess.EF;
using MoreForLess.DataAccess.Entities;
using NLog;

namespace MoreForLess.BusinessLogic.Services
{
    /// <inheritdoc />
    public class CommentService : ICommentService
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<CommentDomainModel> _commentDomainModelValidator;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CommentService"/> class.
        /// </summary>
        /// <param name="context">
        ///     Context of entity framework.
        /// </param>
        /// <param name="mapper">
        ///     Instance of <see cref="Mapper"/>.
        /// </param>
        /// <param name="commentDomainModelValidator">
        ///     Instance of type that implements interface
        ///     <see cref="IValidator{CommentDomainModel}"/>.
        /// </param>
        public CommentService(
            ApplicationDbContext context,
            IMapper mapper,
            IValidator<CommentDomainModel> commentDomainModelValidator)
        {
            this._context = context;
            this._mapper = mapper;
            this._commentDomainModelValidator = commentDomainModelValidator;
        }

        /// <inheritdoc />
        public async Task CreateCommentAsync(CommentDomainModel commentDomainModel)
        {
            try
            {
                _logger.Info($"Looking for good with id: {commentDomainModel.GoodId}");
                await this._context.Goods.SingleAsync(g => g.Id == commentDomainModel.GoodId);
                _logger.Info($"Good with id: {commentDomainModel.GoodId} exists in database.");
            }
            catch (InvalidOperationException ex)
            {
                throw new ArgumentException($"Good with specified id: {commentDomainModel.GoodId} doesn't exist.", ex);
            }

            try
            {
                this._commentDomainModelValidator.ValidateAndThrow(commentDomainModel);
            }
            catch (ValidationException ex)
            {
                throw new ArgumentException($"Error when validating {commentDomainModel}.", ex);
            }

            var comment = this._mapper.Map<Comment>(commentDomainModel);

            _logger.Info("Adding comment to database");
            this._context.Comments.Add(comment);


            _logger.Info("Saving changes to database");
            await this._context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<IReadOnlyCollection<CommentDomainModel>> GetCommentsAsync(int goodId)
        {
            _logger.Info("Getting comments from database.");
            var comments = await this._context.Comments.Where(c => c.GoodId == goodId).ToListAsync();

            if (comments.Count == 0)
            {
                throw new ArgumentException($"Comments for good with id: {goodId} haven't been added yet.");
            }

            return this._mapper.Map<IReadOnlyCollection<CommentDomainModel>>(comments);
        }

        /// <inheritdoc />
        public async Task DeleteCommentsAsync(int goodId)
        {
            try
            {
                _logger.Info($"Looking for good with id: {goodId}");
                await this._context.Goods.SingleAsync(g => g.Id == goodId);
                _logger.Info($"Good with id: {goodId} exists in database.");
            }
            catch (InvalidOperationException ex)
            {
                throw new ArgumentException($"Good with specified id: {goodId} doesn't exist.", ex);
            }

            _logger.Info($"Getting all comments of good with id: {goodId}.");
            var comments = await this._context.Comments.Where(c => c.GoodId == goodId).ToListAsync();

            if (comments.Count == 0)
            {
                throw new ArgumentException($"Comments for good with id: {goodId} haven't been added yet.");
            }

            _logger.Info($"Deleting all comments of good with id: {goodId}.");
            this._context.Comments.RemoveRange(comments);

            _logger.Info("Saving changes to database");
            await this._context.SaveChangesAsync();
        }
    }
}
