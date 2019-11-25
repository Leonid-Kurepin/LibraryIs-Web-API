using System.Threading.Tasks;
using AutoMapper;
using LibraryIS.CommonLayer.DTO;
using LibraryIS.CommonLayer.Exceptions;
using LibraryIS.DAL;
using LibraryIS.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using static System.StringComparison;
using static LibraryIS.BLL.Validators.BookValidator;

namespace LibraryIS.BLL.Helpers
{
    internal class UniquenessInDbCheckerHelper
    {
        internal static async Task CheckMemberInDbAsync(LibraryDbContext dbContext, MemberDto memberDto)
        {
            if (await dbContext.Members
                .AnyAsync(m => m.PassportNumber == memberDto.PassportNumber
                               && m.PassportSeries == memberDto.PassportSeries))
            {
                throw new ResourceConflictException("Member can\'t be added or modified." +
                                                    " There is already a member with such passport information");
            }
        }

        internal static async Task CheckUserInDbAsync(LibraryDbContext dbContext, UserDto userDto)
        {
            if (await dbContext.Users
                .AnyAsync(u => u.Email.Equals(userDto.Email, InvariantCultureIgnoreCase)))
            {
                throw new ResourceConflictException("User can\'t be added or modified." +
                                                    " There is already a user with such email");
            }
        }

        internal static async Task CheckAuthorInDbAsync(LibraryDbContext dbContext, IMapper mapper, BookDto bookDto)
        {
            if (bookDto.Author == null)
            {
                return;
            }

            ValidateAuthorName(bookDto.Author.Name);

            var retrievedAuthor =
                await dbContext.Authors.FirstOrDefaultAsync(
                    a => a.Name.Equals(bookDto.Author.Name, InvariantCultureIgnoreCase));

            // if such Author is already in database
            if (retrievedAuthor != null)
            {
                bookDto.Author = mapper.Map<Author, AuthorDto>(retrievedAuthor);
            }

            // if it is new Author
            else
            {
                var addedAuthor = mapper.Map<AuthorDto, Author>(bookDto.Author);

                await dbContext.Authors.AddAsync(addedAuthor);
                await dbContext.SaveChangesAsync();

                bookDto.Author = mapper.Map<Author, AuthorDto>(addedAuthor);
            }
        }

        internal static async Task CheckPublisherInDbAsync(LibraryDbContext dbContext, IMapper mapper, BookDto bookDto)
        {
            if (bookDto.Publisher == null)
            {
                return;
            }

            ValidatePublisherName(bookDto.Publisher.Name);

            var retrievedPublisher =
                await dbContext.Publishers.FirstOrDefaultAsync(
                    a => a.Name.Equals(bookDto.Publisher.Name, InvariantCultureIgnoreCase));

            // if such Publisher is already in database
            if (retrievedPublisher != null)
            {
                bookDto.Publisher = mapper.Map<Publisher, PublisherDto>(retrievedPublisher);
            }

            // if it is new Publisher
            else
            {
                var addedPublisher = mapper.Map<PublisherDto, Publisher>(bookDto.Publisher);

                await dbContext.Publishers.AddAsync(addedPublisher);
                await dbContext.SaveChangesAsync();

                bookDto.Publisher = mapper.Map<Publisher, PublisherDto>(addedPublisher);
            }
        }
    }
}
