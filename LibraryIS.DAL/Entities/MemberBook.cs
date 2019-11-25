namespace LibraryIS.DAL.Entities
{
    public class MemberBook
    {
        public int MemberId { get; set; }
        public Member Member { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }

        public MemberBook(int memberId, int bookId)
        {
            MemberId = memberId;
            BookId = bookId;
        }
    }
}
