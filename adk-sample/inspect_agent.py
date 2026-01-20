
import sys
import os

# Add mcp_agent to path
sys.path.append(os.path.join(os.path.dirname(__file__), "mcp_agent"))

from agent import root_agent

print("Agent attributes/methods:")
print(dir(root_agent))

try:
    print("\nAgent type:", type(root_agent))
except:
    pass
