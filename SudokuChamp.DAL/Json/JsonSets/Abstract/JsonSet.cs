using System.Text.Json;
using SudokuChamp.API.DAL.Entities.Abstract;

namespace SudokuChamp.API.DAL.Json.JsonSets.Abstract
{
    public abstract class JsonSet<T> where T : class, IIdentifiable
    {
        private string jsonFilePath;
        protected JsonSet(string jsonFilePath)
        {
            this.jsonFilePath = jsonFilePath;
        }

        public async Task<List<T>> GetAll()
        {
            var list = new List<T>();
            var all = ( await File.ReadAllTextAsync(jsonFilePath) ).Trim().Split('\n');
            if (!string.IsNullOrEmpty(all[0]))
            {
                foreach (var item in all)
                {
                    var value = JsonSerializer.Deserialize<T>(item);
                    list.Add(value!);
                }
            }
            return list;
        }

        public async Task Add(T value)
        {
            var strValue = JsonSerializer.Serialize(value);
            await File.AppendAllTextAsync(jsonFilePath, strValue + "\n");
        }

        public async Task Update(Guid id, T value)
        {
            var strValue = JsonSerializer.Serialize(value);
            var all = ( await File.ReadAllTextAsync(jsonFilePath) ).Trim().Split('\n').Select(x => JsonSerializer.Deserialize<T>(x)).ToList();
            File.Delete(jsonFilePath);
            foreach (var item in all)
            {
                if (item!.Id == id)
                {
                    File.AppendAllText(jsonFilePath, strValue + "\n");
                }
                else
                {
                    File.AppendAllText(jsonFilePath, JsonSerializer.Serialize(item) + "\n");
                }
            }
        }
    }
}
