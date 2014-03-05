using System.Linq;
using System.Text;

namespace QuickMGenerate
{

	public delegate Result<TState, TValue> Generator<TState, TValue>(TState input);
}
