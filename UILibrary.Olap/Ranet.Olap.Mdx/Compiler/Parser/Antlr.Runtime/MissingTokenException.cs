/*   
    Copyright (C) 2009 Galaktika Corporation ZAO

    This file is a part of Ranet.UILibrary.Olap
 
    Ranet.UILibrary.Olap is a free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.
      
    You should have received a copy of the GNU General Public License
    along with Ranet.UILibrary.Olap.  If not, see <http://www.gnu.org/licenses/>.
  
    If GPL v.3 is not suitable for your products or company,
    Galaktika Corp provides Ranet.UILibrary.Olap under a flexible commercial license
    designed to meet your specific usage and distribution requirements.
    If you have already obtained a commercial license from Galaktika Corp,
    you can use this file under those license terms.
 
		Moreover you can use this file in source form under original
    "BSD licence" thereunder.
*/
/*
[The "BSD licence"]
Copyright (c) 2007-2008 Johannes Luber
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions
are met:
1. Redistributions of source code MUST RETAIN the above copyright
	 notice, this list of conditions and the following disclaimer.
2. Redistributions in binary form MUST REPRODUCE the above copyright
	 notice, this list of conditions and the following disclaimer in 
	 the documentation and/or other materials provided with the 
	 distribution.
3. The name of the author may not be used to endorse or promote products
	 derived from this software without specific prior WRITTEN permission.

THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;

namespace Antlr.Runtime
{

	/// <summary>
	/// We were expecting a token but it's not found. The current token
	/// is actually what we wanted next. Used for tree node errors too.
	/// </summary>
	public class MissingTokenException : MismatchedTokenException
	{
		private object inserted;

		/// <summary>
		/// Used for remote debugger deserialization
		/// </summary>
		public MissingTokenException()
		{
		}

		public MissingTokenException(int expecting, IIntStream input, object inserted)
			: base(expecting, input)
		{
			this.inserted = inserted;
		}

		public int MissingType
		{
			get { return Expecting; }
		}

		public object Inserted
		{
			get { return inserted; }
			set { inserted = value; }
		}

		public override String ToString()
		{
			if (inserted != null && token != null)
			{
				return "MissingTokenException(inserted " + inserted + " at " + token.Text + ")";
			}
			if (token != null)
			{
				return "MissingTokenException(at " + token.Text + ")";
			}
			return "MissingTokenException";
		}
	}
}
